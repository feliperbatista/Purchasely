using Purchasely.Domain.Entities;

namespace Purchasely.Application.Interfaces;

public interface IRequisitionRepository
{
    Task<Requisition?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Requisition>> GetAllAsync(CancellationToken cancellationToken);
    Task AddAsync(Requisition requisition, CancellationToken cancellationToken);
    Task AddApprovalAsync(Approval approval, CancellationToken cancellationToken);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}