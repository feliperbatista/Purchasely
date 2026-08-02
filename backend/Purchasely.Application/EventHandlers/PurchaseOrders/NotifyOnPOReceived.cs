using MediatR;
using Purchasely.Application.Events.PurchaseOrders;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;

namespace Purchasely.Application.EventHandlers.PurchaseOrders;

public class NotifyOnPOReceived(
    INotificationRepository notificationRepo,
    INotificationService notificationService
) : INotificationHandler<PurchaseOrderReceivedEvent>
{
    public async Task Handle(PurchaseOrderReceivedEvent notification, CancellationToken cancellationToken)
    {
        var newNotification = Notification.Create(
            notification.CreatedBy,
            "Purchase Order Received",
            $"Requisition #{notification.PoNumber} was received.",
            "info",
            notification.PurchaseOrderId,
            "PurchaseOrder"
            );

        await notificationRepo.AddAsync(newNotification, cancellationToken);

        await notificationRepo.SaveChangesAsync(cancellationToken);

        await notificationService.SendToUsersAsync(
            [notification.ReceiverId, notification.CreatedBy],
            new NotificationPayload(
                Title: "Purchase Order Received",
                Message: $"Purchase Order {notification.PoNumber} has been received.",
                Type: "success",
                EntityId: notification.PurchaseOrderId,
                EntityType: "PurchaseOrder"
            ),
            cancellationToken);
    }
}