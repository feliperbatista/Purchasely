using MediatR;
using Purchasely.Application.Events.Requisitions;
using Purchasely.Application.Interfaces;
using Purchasely.Application.Messages.Emails;

namespace Purchasely.Application.EventHandlers.Requisitions;

public class SendEmailOnRequisitionRejected(
    IBus bus
) : INotificationHandler<RequisitionRejectedEvent>
{
    public async Task Handle(RequisitionRejectedEvent notification, CancellationToken cancellationToken)
    {
        await bus.PublishAsync(new RequisitionRejectedEmailMessage(
            notification.RequisitionId,
            notification.RequisitionNumber,
            notification.RequesterEmail,
            notification.RequesterName,
            notification.Reason
        ), cancellationToken);
    }
}