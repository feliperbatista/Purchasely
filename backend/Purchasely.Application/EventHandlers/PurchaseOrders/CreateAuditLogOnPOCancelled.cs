using MediatR;
using Purchasely.Application.Events.PurchaseOrders;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;

namespace Purchasely.Application.EventHandlers.PurchaseOrders;

public class CreateAuditLogOnPOCancelled(
    IAuditLogRepository auditRepo
) : INotificationHandler<PurchaseOrderCancelledEvent>
{
    public async Task Handle(PurchaseOrderCancelledEvent notification, CancellationToken cancellationToken)
    {
        var log = AuditLog.Create(
            "Purchase Order",
            notification.PurchaseOrderId,
            "Purchase order cancelled",
            notification.CancellerId,
            notification.CancelledAt
        );

        await auditRepo.AddAsync(log, cancellationToken);
        await auditRepo.SaveChangesAsync(cancellationToken);
    }
}