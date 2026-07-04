using MediatR;
using Purchasely.Application.Events.PurchaseOrders;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;

namespace Purchasely.Application.EventHandlers.PurchaseOrders;

public class CreateAuditLogOnPOIssued(
    IAuditLogRepository auditRepo
) : INotificationHandler<PurchaseOrderIssuedEvent>
{
    public async Task Handle(PurchaseOrderIssuedEvent notification, CancellationToken cancellationToken)
    {
        var log = AuditLog.Create(
            "Purchase Order",
            notification.PurchaseOrderId,
            "Purchase order issued",
            notification.IssuerId,
            notification.IssuedAt
        );

        await auditRepo.AddAsync(log, cancellationToken);
        await auditRepo.SaveChangesAsync(cancellationToken);
    }
}