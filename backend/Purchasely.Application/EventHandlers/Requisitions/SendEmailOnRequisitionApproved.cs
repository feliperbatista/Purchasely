using MediatR;
using Purchasely.Application.Events.Requisitions;
using Purchasely.Application.Interfaces;
using Purchasely.Application.Messages.Emails;

namespace Purchasely.Application.EventHandlers.Requisitions;

public class SendEmailOnRequisitionApproved(
    IBus bus
) : INotificationHandler<RequisitionApprovedEvent>
{
    public async Task Handle(RequisitionApprovedEvent notification, CancellationToken cancellationToken)
    {
        await bus.PublishAsync(new RequisitionApprovedEmailMessage(
            notification.RequisitionId,
            notification.RequisitionNumber,
            notification.RequesterEmail,
            notification.RequesterName,
            notification.ApproverName
        ), cancellationToken);
    }
}