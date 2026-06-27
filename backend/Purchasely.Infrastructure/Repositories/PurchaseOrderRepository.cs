
using Microsoft.EntityFrameworkCore;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;
using Purchasely.Infrastructure.Persistence;

namespace Purchasely.Infrastructure.Repositories;

public class PurchaseOrderRepository(AppDbContext context) : IPurchaseOrderRepository
{
    public async Task AddAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken)
    {
        await context.PurchaseOrders.AddAsync(purchaseOrder, cancellationToken);
    }

    public async Task<List<PurchaseOrder>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.PurchaseOrders
            .Include(p => p.Supplier)
            .Include(p => p.Lines)
                .ThenInclude(l => l.Product)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<PurchaseOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.PurchaseOrders
            .Include(p => p.Lines)
                .ThenInclude(l => l.Product)
            .Include(p => p.Creator)
            .Include(p => p.Supplier)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}