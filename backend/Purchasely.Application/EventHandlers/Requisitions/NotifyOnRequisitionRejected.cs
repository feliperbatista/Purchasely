using MediatR;
using Purchasely.Application.Events.Requisitions;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;

namespace Purchasely.Application.EventHandlers.Requisitions;

public class NotifyOnRequisitionRejected(
    INotificationRepository notificationRepo,
    INotificationService notificationService
) : INotificationHandler<RequisitionRejectedEvent>
{
    public async Task Handle(RequisitionRejectedEvent notification, CancellationToken cancellationToken)
    {
        var newNotification = Notification.Create(notification.RequesterId, "Requisition Rejected", $"Requisition #{notification.RequisitionNumber} was Rejected. Reason: {notification.Reason}");

        await notificationRepo.AddAsync(newNotification, cancellationToken);

        await notificationRepo.SaveChangesAsync(cancellationToken);

        await notificationService.SendToUserAsync(
            notification.RequesterId,
            new NotificationPayload(
                Title: "Requisition Rejected",
                Message: $"Your requisition #{notification.RequisitionNumber} was rejected. Reason: {notification.Reason}.",
                Type: "error",
                EntityId: notification.RequisitionId,
                EntityType: "Requisition"
            ),
            cancellationToken);
    }
}