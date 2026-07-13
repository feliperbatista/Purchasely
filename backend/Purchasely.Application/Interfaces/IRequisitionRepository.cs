using Purchasely.Domain.Entities;
using Purchasely.Domain.Enums;

namespace Purchasely.Application.Interfaces;

public interface IRequisitionRepository
{
    Task<Requisition?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Requisition>> GetAllAsync(
        int page,
        int pageSize,
        RequisitionStatus? status = null,
        Priority? priority = null,
        DateTime? from = null,
        DateTime? to = null,
        CancellationToken cancellationToken = default
    );

    Task<int> CountAsync(CancellationToken cancellationToken);
    Task<int> CountByStatusAsync(RequisitionStatus[] status, CancellationToken cancellationToken);
    Task AddAsync(Requisition requisition, CancellationToken cancellationToken);
    Task AddApprovalAsync(Approval approval, CancellationToken cancellationToken);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}