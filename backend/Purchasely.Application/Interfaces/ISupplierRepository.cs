using Purchasely.Domain.Entities;

namespace Purchasely.Application.Interfaces;

public interface ISupplierRepository
{
    Task<Supplier?> GetByIdAsync(Guid id);
    Task<List<Supplier>> GetAllAsync();
    Task AddAsync(Supplier supplier);
    void Update(Supplier supplier);
    void Delete(Supplier supplier);
    Task SaveChangesAsync();
}