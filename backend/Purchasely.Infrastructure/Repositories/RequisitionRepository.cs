using Microsoft.EntityFrameworkCore;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;
using Purchasely.Infrastructure.Persistence;

namespace Purchasely.Infrastructure.Repositories;

public class RequisitionRepository(AppDbContext context) : IRequisitionRepository
{
    public async Task AddAsync(Requisition requisition, CancellationToken cancellationToken)
    {
        await context.Requisitions.AddAsync(requisition, cancellationToken);
    }

    public async Task<List<Requisition>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Requisitions
            .AsNoTracking()
            .Include(r => r.Lines)
                .ThenInclude(l => l.Product)
            .Include(r => r.Requester)
            .ToListAsync(cancellationToken);
    }

    public async Task<Requisition?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Requisitions
            .Include(r => r.Lines)
                .ThenInclude(l => l.Product)
            .Include(r => r.Requester)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }

    public void Update(Requisition requisition)
    {
        context.Requisitions.Update(requisition);
    }
}