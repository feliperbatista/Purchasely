using MediatR;
using Purchasely.Application.Events.PurchaseOrders;
using Purchasely.Application.Interfaces;
using Purchasely.Application.Messages.Emails;

namespace Purchasely.Application.EventHandlers.PurchaseOrders;

public class SendEmailOnPurchaseOrderIssued(
    IBus bus
) : INotificationHandler<PurchaseOrderIssuedEvent>
{
    public async Task Handle(PurchaseOrderIssuedEvent notification, CancellationToken cancellationToken)
    {
        await bus.PublishAsync(new PurchaseOrderIssuedEmailMessage(
            notification.PurchaseOrderId,
            notification.PurchaseOrderNumber,
            notification.SupplierEmail,
            notification.SupplierName,
            notification.TotalAmount
        ), cancellationToken);
    }
}