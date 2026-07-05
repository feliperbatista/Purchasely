using MediatR;
using Purchasely.Application.Events.PurchaseOrders;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;

namespace Purchasely.Application.EventHandlers.PurchaseOrders;

public class NotifyOnPOReceived(
    INotificationRepository notificationRepo,
    IPurchaseOrderRepository purchaseOrderRepo,
    INotificationService notificationService
) : INotificationHandler<PurchaseOrderReceivedEvent>
{
    public async Task Handle(PurchaseOrderReceivedEvent notification, CancellationToken cancellationToken)
    {
        var po = await purchaseOrderRepo.GetByIdAsync(notification.PurchaseOrderId, cancellationToken);
        if (po is null)
            return;

        var newNotification = Notification.Create(po.CreatedBy, "Purchase Order Received", $"Requisition #{po.Number} was received.");

        await notificationRepo.AddAsync(newNotification, cancellationToken);

        await notificationRepo.SaveChangesAsync(cancellationToken);

        await notificationService.SendToUsersAsync(
            [notification.ReceiverId, po.CreatedBy],
            new NotificationPayload(
                Title: "Purchase Order Received",
                Message: $"Purchase Order {po.Number} has been received.",
                Type: "success",
                EntityId: notification.PurchaseOrderId,
                EntityType: "PurchaseOrder"
            ),
            cancellationToken);
    }
}