using MediatR;

namespace Purchasely.Application.Events.PurchaseOrders;

public record PurchaseOrderReceivedEvent(
    Guid PurchaseOrderId,
    Guid CreatedBy,
    Guid ReceiverId,
    DateTime ReceivedAt,
    int PoNumber
) : INotification;

