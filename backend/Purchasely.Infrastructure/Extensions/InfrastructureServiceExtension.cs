using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Purchasely.Application.Interfaces;
using Purchasely.Infrastructure.Messaging;
using Purchasely.Infrastructure.Persistence;
using Purchasely.Infrastructure.Repositories;
using Purchasely.Infrastructure.Services;
using RabbitMQ.Client;

namespace Purchasely.Infrastructure.Extensions;

public static class InfrastructureServiceExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(config =>
        {
            config.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
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
                UserName = configuration["RabbitMQ:UserName"]!,
                Password = configuration["RabbitMQ:Password"]!,
            };
            return factory.CreateConnectionAsync().GetAwaiter().GetResult();
        });

        services.AddScoped<IBus, RabbitMqBus>();
        services.AddHostedService<EmailConsumerWorker>();
        services.AddScoped<IEmailService, EmailService>();
    }
}