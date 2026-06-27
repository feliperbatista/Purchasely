using Purchasely.Domain.Entities;

namespace Purchasely.Application.Interfaces;

public interface ISupplierRepository
{
    Task<Supplier?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Supplier?> GetByTaxNumberAsync(string taxNumber, CancellationToken cancellationToken);
    Task<List<Supplier>> GetAllAsync(CancellationToken cancellationToken);
    Task AddAsync(Supplier supplier, CancellationToken cancellationToken);
    void Delete(Supplier supplier);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}