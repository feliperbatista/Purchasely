
using Microsoft.EntityFrameworkCore;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;
using Purchasely.Domain.Enums;
using Purchasely.Infrastructure.Persistence;

namespace Purchasely.Infrastructure.Repositories;

public class PurchaseOrderRepository(AppDbContext context) : IPurchaseOrderRepository
{
    public async Task AddAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken)
    {
        await context.PurchaseOrders.AddAsync(purchaseOrder, cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken)
    {
        return await context.PurchaseOrders.CountAsync(cancellationToken);
    }

    public async Task<List<PurchaseOrder>> GetAllAsync(int page, int pageSize, PurchaseOrderStatus? status = null, DateTime? from = null, DateTime? to = null, CancellationToken cancellationToken = default)
    {
        return await context.PurchaseOrders
            .AsNoTracking()
            .Where(r => !status.HasValue || r.Status == status.Value)
            .Where(r => !from.HasValue || r.CreatedAt >= from.Value.Date)
            .Where(r => !to.HasValue || r.CreatedAt <= to.Value.Date.AddDays(1).AddTicks(-1))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(p => p.Supplier)
            .Include(p => p.Lines)
                .ThenInclude(l => l.Product)
            .ToListAsync(cancellationToken);
    }

    public async Task<PurchaseOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.PurchaseOrders
            .Include(p => p.Lines)
                .ThenInclude(l => l.Product)
            .Include(p => p.Creator)
            .Include(p => p.Supplier)
            .Include(p => p.Documents)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}