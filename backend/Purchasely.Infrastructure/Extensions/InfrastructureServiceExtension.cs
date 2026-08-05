using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Purchasely.Application.Interfaces;
using Purchasely.Infrastructure.Messaging;
using Purchasely.Infrastructure.Persistence;
using Purchasely.Infrastructure.Repositories;
using Purchasely.Infrastructure.Services;
using RabbitMQ.Client;
using StackExchange.Redis;

namespace Purchasely.Infrastructure.Extensions;

public static class InfrastructureServiceExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(config =>
        {
            config.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration["Redis:ConnectionString"];
        });

        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISupplierProductRepository, SupplierProductRepository>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IRequisitionRepository, RequisitionRepository>();
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
        services.AddScoped<IFileStorageService, AzureBlobStorageService>();

        services.AddSingleton(_ =>
        {
            var factory = new ConnectionFactory
            {
                HostName = configuration["RabbitMQ:Host"]!,
                UserName = configuration["RabbitMQ:Username"]!,
                Password = configuration["RabbitMQ:Password"]!,
            };
            return factory.CreateConnectionAsync().GetAwaiter().GetResult();
        });
        services.AddSingleton<IConnectionMultiplexer>(_ => 
            ConnectionMultiplexer.Connect(configuration["Redis:ConnectionString"]!));

        services.AddScoped<IBus, RabbitMqBus>();
        services.AddHostedService<EmailConsumerWorker>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICacheService, RedisCacheService>();
    }
}