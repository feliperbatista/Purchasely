using Purchasely.Domain.Entities;

namespace Purchasely.Application.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Product>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken);
    Task<List<Product>> GetAllAsync(CancellationToken cancellationToken);
    Task AddAsync(Product product, CancellationToken cancellationToken);
    void Delete(Product product);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}