using MediatR;
using Purchasely.Application.Events.Requisitions;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;

namespace Purchasely.Application.EventHandlers.Requisitions;

public class NotifyRequesterOnRequisitionApproved(
    INotificationRepository notificationRepo,
    IRequisitionRepository requisitionRepo
) : INotificationHandler<RequisitionApprovedEvent>
{
    public async Task Handle(RequisitionApprovedEvent notification, CancellationToken cancellationToken)
    {
        var requisition = await requisitionRepo.GetByIdAsync(notification.RequisitionId, cancellationToken);
        if (requisition is null)
            return;

        var newNotification = Notification.Create(requisition.RequesterId, "Requisition Approved", $"Requisition #{requisition.Number} was approved.");

        await notificationRepo.AddAsync(newNotification, cancellationToken);

        await notificationRepo.SaveChangesAsync(cancellationToken);
    }
}