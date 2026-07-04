using MediatR;

namespace Purchasely.Application.Events.PurchaseOrders;

public record PurchaseOrderReceivedEvent(
    Guid PurchaseOrderId,
    Guid ReceiverId,
    DateTime ReceivedAt
) : INotification;

