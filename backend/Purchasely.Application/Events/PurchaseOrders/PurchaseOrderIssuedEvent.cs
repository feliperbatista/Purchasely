using MediatR;

namespace Purchasely.Application.Events.PurchaseOrders;

public record PurchaseOrderIssuedEvent(
    Guid PurchaseOrderId,
    Guid IssuerId,
    DateTime IssuedAt
) : INotification;

