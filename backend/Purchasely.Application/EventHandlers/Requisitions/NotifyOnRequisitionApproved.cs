using MediatR;
using Purchasely.Application.Events.Requisitions;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;

namespace Purchasely.Application.EventHandlers.Requisitions;

public class NotifyOnRequisitionApproved(
    INotificationRepository notificationRepo,
    INotificationService notificationService
) : INotificationHandler<RequisitionApprovedEvent>
{
    public async Task Handle(RequisitionApprovedEvent notification, CancellationToken cancellationToken)
    {
        var newNotification = Notification.Create(notification.RequesterId, "Requisition Approved", $"Requisition #{notification.RequisitionNumber} was approved.");

        await notificationRepo.AddAsync(newNotification, cancellationToken);

        await notificationRepo.SaveChangesAsync(cancellationToken);

        await notificationService.SendToUserAsync(
            notification.RequesterId,
            new NotificationPayload(
                Title: "Requisition Approved",
                Message: $"Your requisition #{notification.RequisitionNumber} was approved by {notification.ApproverName}.",
                Type: "success",
                EntityId: notification.RequisitionId,
                EntityType: "Requisition"
            ),
            cancellationToken);
    }
}