using Microsoft.EntityFrameworkCore;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;
using Purchasely.Infrastructure.Persistence;

namespace Purchasely.Infrastructure.Repositories;

public class ProductRepository(AppDbContext context) : IProductRepository
{
    public async Task AddAsync(Product product, CancellationToken cancellationToken)
    {
        await context.Products.AddAsync(product, cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken)
    {
        return await context.Products.CountAsync(cancellationToken);
    }

    public void Delete(Product product)
    {
        context.Products.Remove(product);
    }

    public async Task<List<Product>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        return await context.Products
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Products
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<Product>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken)
    {
        return await context.Products
            .Where(x => ids.Contains(x.Id))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}