using MediatR;
using Purchasely.Application.Events.Requisitions;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;

namespace Purchasely.Application.EventHandlers.Requisitions;

public class NotifyOnRequisitionSubmitted(
    INotificationRepository notificationRepo,
    IRequisitionRepository requisitionRepo,
    INotificationService notificationService
) : INotificationHandler<RequisitionSubmittedEvent>
{
    public async Task Handle(RequisitionSubmittedEvent notification, CancellationToken cancellationToken)
    {
        var requisition = await requisitionRepo.GetByIdAsync(notification.RequisitionId, cancellationToken);
        if (requisition is null)
            return;

        var newNotification = Notification.Create(
            requisition.RequesterId,
            "Requisition Submitted",
            $"Requisition #{requisition.Number} was submitted");

        await notificationRepo.AddAsync(newNotification, cancellationToken);

        await notificationRepo.SaveChangesAsync(cancellationToken);

        await notificationService.SendToUsersAsync(
            notification.ApproverIds,
            new NotificationPayload(
                Title: "New Requisition Awaiting Approval",
                Message: $"Requisition #{notification.RequisitionNumber} requires your approval.",
                Type: "info",
                EntityId: notification.RequisitionId,
                EntityType: "Requsition"
            ),
            cancellationToken
        );
    }
}