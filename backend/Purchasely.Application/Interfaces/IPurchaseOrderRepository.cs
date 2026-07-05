using Purchasely.Domain.Entities;
using Purchasely.Domain.Enums;

namespace Purchasely.Application.Interfaces;

public interface IPurchaseOrderRepository
{
    Task<PurchaseOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<PurchaseOrder>> GetAllAsync(
        int page,
        int pageSize,
        PurchaseOrderStatus? status = null,
        DateTime? from = null,
        DateTime? to = null,
        CancellationToken cancellationToken = default
    );
    Task<int> CountAsync(CancellationToken cancellationToken);
    Task AddAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}