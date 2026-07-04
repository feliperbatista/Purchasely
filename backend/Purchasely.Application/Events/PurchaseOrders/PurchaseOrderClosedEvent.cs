using MediatR;

namespace Purchasely.Application.Events.PurchaseOrders;

public record PurchaseOrderClosedEvent(
    Guid PurchaseOrderId,
    Guid CloserId,
    DateTime ClosedAt
) : INotification;

