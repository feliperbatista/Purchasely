using Purchasely.Domain.Entities;

namespace Purchasely.Application.Interfaces;

public interface IRequisitionRepository
{
    Task<Requisition?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Requisition>> GetAllAsync(CancellationToken cancellationToken);
    Task AddAsync(Requisition requisition, CancellationToken cancellationToken);
    void Update(Requisition requisition);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}