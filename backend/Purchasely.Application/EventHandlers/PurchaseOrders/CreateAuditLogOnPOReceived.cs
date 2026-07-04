using MediatR;
using Purchasely.Application.Events.PurchaseOrders;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;

namespace Purchasely.Application.EventHandlers.PurchaseOrders;

public class CreateAuditLogOnPOReceived(
    IAuditLogRepository auditRepo
) : INotificationHandler<PurchaseOrderReceivedEvent>
{
    public async Task Handle(PurchaseOrderReceivedEvent notification, CancellationToken cancellationToken)
    {
        var log = AuditLog.Create(
            "Purchase Order",
            notification.PurchaseOrderId,
            "Purchase order received",
            notification.ReceiverId,
            notification.ReceivedAt
        );

        await auditRepo.AddAsync(log, cancellationToken);
        await auditRepo.SaveChangesAsync(cancellationToken);
    }
}