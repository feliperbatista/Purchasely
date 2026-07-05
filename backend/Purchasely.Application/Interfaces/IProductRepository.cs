using Purchasely.Domain.Entities;

namespace Purchasely.Application.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Product>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken);
    Task<List<Product>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken);
    Task<int> CountAsync(CancellationToken cancellationToken);
    Task AddAsync(Product product, CancellationToken cancellationToken);
    void Delete(Product product);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}