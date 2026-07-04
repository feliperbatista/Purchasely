using MediatR;
using Purchasely.Application.Events.Requisitions;
using Purchasely.Application.Interfaces;
using Purchasely.Application.Messages.Emails;

namespace Purchasely.Application.EventHandlers.Requisitions;

public class SendEmailOnRequisitionSubmitted(
    IBus bus
) : INotificationHandler<RequisitionSubmittedEvent>
{
    public async Task Handle(RequisitionSubmittedEvent notification, CancellationToken cancellationToken)
    {
        await bus.PublishAsync(new RequisitionSubmittedEmailMessage(
            notification.RequisitionId,
            notification.RequisitionNumber,
            notification.RequesterName,
            notification.ApproverEmails,
            notification.SubmittedAt
        ), cancellationToken);
    }
}