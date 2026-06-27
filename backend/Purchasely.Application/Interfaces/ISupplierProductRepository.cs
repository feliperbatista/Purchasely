using Purchasely.Domain.Entities;

namespace Purchasely.Application.Interfaces;

public interface ISupplierProductRepository
{
    Task<bool> ExistsForSupplierAndProduct(Guid supplierId, Guid productId, CancellationToken cancellationToken);
    Task<SupplierProduct?> GetByIdsAsync(Guid supplierId, Guid productId, CancellationToken cancellationToken);
    Task AddAsync(SupplierProduct product, CancellationToken cancellationToken);
    void Delete(SupplierProduct product);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}