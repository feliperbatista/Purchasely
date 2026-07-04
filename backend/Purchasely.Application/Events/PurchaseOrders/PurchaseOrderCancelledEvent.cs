using MediatR;

namespace Purchasely.Application.Events.PurchaseOrders;

public record PurchaseOrderCancelledEvent(
    Guid PurchaseOrderId,
    Guid CancellerId,
    DateTime CancelledAt
) : INotification;

