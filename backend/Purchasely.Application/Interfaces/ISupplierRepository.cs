using Purchasely.Domain.Entities;

namespace Purchasely.Application.Interfaces;

public interface ISupplierRepository
{
    Task<Supplier?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Supplier?> GetByTaxNumberAsync(string taxNumber, CancellationToken cancellationToken);
    Task<List<Supplier>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken);
    Task<int> CountAsync(CancellationToken cancellationToken);
    Task<List<Supplier>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken);
    Task AddAsync(Supplier supplier, CancellationToken cancellationToken);
    void Delete(Supplier supplier);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}