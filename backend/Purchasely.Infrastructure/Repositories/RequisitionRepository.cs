using Microsoft.EntityFrameworkCore;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;
using Purchasely.Domain.Enums;
using Purchasely.Infrastructure.Persistence;

namespace Purchasely.Infrastructure.Repositories;

public class RequisitionRepository(AppDbContext context) : IRequisitionRepository
{
    public async Task AddApprovalAsync(Approval approval, CancellationToken cancellationToken)
    {
        await context.Approvals.AddAsync(approval, cancellationToken);
    }

    public async Task AddAsync(Requisition requisition, CancellationToken cancellationToken)
    {
        await context.Requisitions.AddAsync(requisition, cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken)
    {
        return await context.Requisitions.CountAsync(cancellationToken);
    }

    public async Task<int> CountByStatusAsync(RequisitionStatus[] status, CancellationToken cancellationToken)
    {
        return await context.Requisitions.CountAsync(r => status.Contains(r.Status), cancellationToken);
    }

    public async Task<List<Requisition>> GetAllAsync(int page, int pageSize, RequisitionStatus? status = null, Priority? priority = null, DateTime? from = null, DateTime? to = null, CancellationToken cancellationToken = default)
    {
        return await context.Requisitions
            .AsNoTracking()
            .Where(r => !status.HasValue || r.Status == status.Value)
            .Where(r => !priority.HasValue || r.Priority == priority.Value)
            .Where(r => !from.HasValue || r.CreatedAt >= from.Value.Date)
            .Where(r => !to.HasValue || r.CreatedAt <= to.Value.Date.AddDays(1).AddTicks(-1))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
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
            .Include(r => r.Approvals)
                .ThenInclude(a => a.Approver)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}