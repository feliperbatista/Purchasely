using Microsoft.EntityFrameworkCore;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;
using Purchasely.Infrastructure.Persistence;

namespace Purchasely.Infrastructure.Repositories;

public class SupplierRepository(AppDbContext context) : ISupplierRepository
{
    public async Task AddAsync(Supplier supplier, CancellationToken cancellationToken)
    {
        await context.Suppliers.AddAsync(supplier, cancellationToken);
    }

    public void Delete(Supplier supplier)
    {
        context.Suppliers.Remove(supplier);
    }

    public async Task<List<Supplier>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Suppliers
            .AsNoTracking()

            .ToListAsync(cancellationToken);
    }

    public async Task<Supplier?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Suppliers
            .Include(s => s.Products)
            .ThenInclude(p => p.Product)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<Supplier>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken)
    {
        return await context.Suppliers
            .Where(x => ids.Contains(x.Id))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Supplier?> GetByTaxNumberAsync(string taxNumber, CancellationToken cancellationToken)
    {
        return await context.Suppliers
            .FirstOrDefaultAsync(x => x.TaxNumber == taxNumber, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}