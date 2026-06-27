using MediatR;
using Purchasely.Application.Events.Requisitions;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;

namespace Purchasely.Application.EventHandlers.Requisitions;

public class NotifyRequesterOnRequisitionRejected(
    INotificationRepository notificationRepo,
    IRequisitionRepository requisitionRepo
) : INotificationHandler<RequisitionRejectedEvent>
{
    public async Task Handle(RequisitionRejectedEvent notification, CancellationToken cancellationToken)
    {
        var requisition = await requisitionRepo.GetByIdAsync(notification.RequisitionId, cancellationToken);
        if (requisition is null)
            return;

        var newNotification = Notification.Create(requisition.RequesterId, "Requisition Rejected", $"Requisition #{requisition.Number} was Rejected. Reason: {notification.Reason}");

        await notificationRepo.AddAsync(newNotification, cancellationToken);

        await notificationRepo.SaveChangesAsync(cancellationToken);
    }
}