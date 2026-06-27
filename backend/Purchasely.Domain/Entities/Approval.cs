namespace Purchasely.Domain.Entities;

public class Approval
{
    public Guid Id { get; set; }
    public Guid RequisitionId { get; set; }
    public Requisition Requisition { get; set; } = null!;
    public User Approver { get; set; } = null!;
    public Guid ApproverId { get; set; }
    public DateTime ActionedAt { get; set; }

    private Approval() {}

    internal static Approval Create(Guid requisitionId, Guid approverId)
    {
        return new Approval
        {
            Id = Guid.NewGuid(),
            RequisitionId = requisitionId,
            ApproverId = approverId,
            ActionedAt = DateTime.UtcNow
        };
    }
}