using Purchasely.Domain.Entities;

namespace Purchasely.Application.Interfaces;

public interface IPurchaseOrderRepository
{
    Task<PurchaseOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<PurchaseOrder>> GetAllAsync(CancellationToken cancellationToken);
    Task AddAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}