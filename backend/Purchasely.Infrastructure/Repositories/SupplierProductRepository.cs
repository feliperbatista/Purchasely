using Microsoft.EntityFrameworkCore;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;
using Purchasely.Infrastructure.Persistence;

namespace Purchasely.Infrastructure.Repositories;

public class SupplierProductRepository(AppDbContext context) : ISupplierProductRepository
{
    public async Task AddAsync(SupplierProduct product, CancellationToken cancellationToken)
    {
        await context.AddAsync(product, cancellationToken);
    }

    public void Delete(SupplierProduct product)
    {
        context.Remove(product);
    }

    public async Task<bool> ExistsForSupplierAndProduct(Guid supplierId, Guid productId, CancellationToken cancellationToken)
    {
        return await context.SupplierProducts
            .AnyAsync(x => x.SupplierId == supplierId && x.ProductId == productId, cancellationToken);
    }

    public async Task<SupplierProduct?> GetByIdAsync(Guid supplierProductId, CancellationToken cancellationToken)
    {
        return await context.SupplierProducts
            .FirstOrDefaultAsync(x => x.Id == supplierProductId, cancellationToken);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}