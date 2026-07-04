using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Purchasely.Application.Interfaces;
using Purchasely.Application.Messages.Emails;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Purchasely.Infrastructure.Messaging;

public class EmailConsumerWorker(IConnection connection, IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        var queues = new[]
        {
            nameof(RequisitionSubmittedEmailMessage),
            nameof(RequisitionApprovedEmailMessage),
            nameof(RequisitionRejectedEmailMessage),
            nameof(PurchaseOrderIssuedEmailMessage),
        };

        foreach (var queue in queues)
        {
            await channel.QueueDeclareAsync(
                queue: queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                cancellationToken: stoppingToken
            );
        }

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            var body = Encoding.UTF8.GetString(ea.Body.ToArray());
            var queueName = ea.RoutingKey;

            using var scope = scopeFactory.CreateScope();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            try
            {
                await DispatchAsync(queueName, body, emailService, stoppingToken);
                await channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
            }
            catch
            {
                await channel.BasicNackAsync(ea.DeliveryTag, false, requeue: true, stoppingToken);
            }
        };

        foreach(var queue in queues)
            await channel.BasicConsumeAsync(queue, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private static async Task DispatchAsync(string queue, string body, IEmailService emailService, CancellationToken ct)
    {
        if (queue == nameof(RequisitionSubmittedEmailMessage))
        {
            var msg = JsonSerializer.Deserialize<RequisitionSubmittedEmailMessage>(body)!;
            foreach(var approverEmail in msg.ApproverEmails)
            {
                await emailService.SendAsync(
                    approverEmail,
                    $"Requisition #{msg.RequisitionNumber} Awaiting Approval",
                    $"<p> Requisition <strong>#{msg.RequisitionNumber}</strong> submitted by {msg.RequesterName} is awaiting approval.</p>",
                    ct
                );
            }
        }
        else if (queue == nameof(RequisitionApprovedEmailMessage))
        {
            var msg = JsonSerializer.Deserialize<RequisitionApprovedEmailMessage>(body)!;
            await emailService.SendAsync(
                msg.RequesterEmail,
                $"Requisition #{msg.RequisitionNumber} Approved",
                $"""
                    <p>Hi {msg.RequesterName},</p>
                    <p>Your requisition <strong>#{msg.RequisitionNumber}</strong> has been approved by {msg.ApproverName}.</p>
                """,
                ct
            );
        }
        else if (queue == nameof(RequisitionRejectedEmailMessage))
        {
            var msg = JsonSerializer.Deserialize<RequisitionRejectedEmailMessage>(body)!;
            await emailService.SendAsync(
                msg.RequesterEmail,
                $"Requisition #{msg.RequisitionNumber} Rejected",
                $"""
                    <p>Hi {msg.RequesterName},</p>
                    <p>Your requisition <strong>#{msg.RequisitionNumber}</strong> has been rejected with the following reason: </p>
                    <p>{msg.Reason}</p>
                """,
                ct
            );
        }
        else if (queue == nameof(PurchaseOrderIssuedEmailMessage))
        {
            var msg = JsonSerializer.Deserialize<PurchaseOrderIssuedEmailMessage>(body)!;
            await emailService.SendAsync(
                msg.SupplierEmail,
                $"Purchase Order {msg.PoNumber}",
                $"""
                    <p>Dear {msg.SupplierName},</p>
                    <p>Please find attached Purchase Order <strong>{msg.PoNumber}</strong> for <strong>{msg.TotalAmount:C}</strong>.</p>
                """,
                ct);
        }
    }
}