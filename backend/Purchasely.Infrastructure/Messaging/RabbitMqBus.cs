using System.Text;
using System.Text.Json;
using Purchasely.Application.Interfaces;
using RabbitMQ.Client;

namespace Purchasely.Infrastructure.Messaging;

public class RabbitMqBus(IConnection connection) : IBus
{
    public async Task PublishAsync<T>(T message, CancellationToken cancellationToken) where T : class
    {
        using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        var queueName = typeof(T).Name;

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken
        );

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var props = new BasicProperties { Persistent = true };

        await channel.BasicPublishAsync(
            exchange: "",
            routingKey: queueName,
            mandatory: false,
            basicProperties: props,
            body: body,
            cancellationToken: cancellationToken
        );
    }
}