using Microsoft.EntityFrameworkCore;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;
using Purchasely.Infrastructure.Persistence;

namespace Purchasely.Infrastructure.Repositories;

public class SupplierRepository(AppDbContext context) : ISupplierRepository
{
    public async Task AddAsync(Supplier supplier)
    {
        await context.Suppliers.AddAsync(supplier);
    }

    public void Delete(Supplier supplier)
    {
        context.Suppliers.Remove(supplier);
    }

    public async Task<List<Supplier>> GetAllAsync()
    {
        return await context.Suppliers
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Supplier?> GetByIdAsync(Guid id)
    {
        return await context.Suppliers
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public void Update(Supplier supplier)
    {
        context.Suppliers.Update(supplier);
    }
}