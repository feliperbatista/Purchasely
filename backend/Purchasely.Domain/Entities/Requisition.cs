using Purchasely.Domain.Enums;

namespace Purchasely.Domain.Entities;

public class Requisition
{
    public Guid Id { get; set; }
    public int Number { get; set; }
    public RequisitionStatus Status { get; set; } = RequisitionStatus.Draft;
    public Priority Priority { get; set; } = Priority.Normal;
    public string? Justification { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public required Guid RequesterId { get; set; }
    public User Requester { get; set; } = null!;
    public ICollection<RequisitionLine> Lines { get; set; } = [];
    public ICollection<Approval> Approvals { get; set; } = [];

    private Requisition() {}

    public static Requisition Create(
        RequisitionStatus status,
        Priority priority,
        string? justification,
        Guid requesterId,
        List<RequisitionLine> lines)
    {
        return new Requisition
        {
            Status = status,
            Priority = priority,
            Justification = justification,
            RequesterId = requesterId,
            CreatedAt = DateTime.UtcNow,
            Lines = lines
        };
    }

    public void Approve(Guid approverId)
    {
        Status = RequisitionStatus.Approved;
        Approvals.Add(Approval.Create(Id, approverId));
    }

    public void RemoveApproval(Approval approval)
    {
        Status = RequisitionStatus.Submitted;
        Approvals.Remove(approval);
    }

    public void Reject()
    {
        Status = RequisitionStatus.Rejected;
    }

    public void Submit()
    {
        Status = RequisitionStatus.Submitted;
        SubmittedAt = DateTime.UtcNow;
    }

    public void ConvertToPO()
    {
        Status = RequisitionStatus.ConvertedToPO;
    }

    public bool CanTransitionTo(RequisitionStatus newStatus)
    {
        return Status switch
        {
            RequisitionStatus.Draft =>
                newStatus == RequisitionStatus.Submitted,

            RequisitionStatus.Submitted =>
                newStatus == RequisitionStatus.Approved ||
                newStatus == RequisitionStatus.Rejected,

            RequisitionStatus.Approved =>
                newStatus == RequisitionStatus.ConvertedToPO,

            RequisitionStatus.Rejected =>
                false,

            RequisitionStatus.ConvertedToPO =>
                false,

            _ => false
        };
    }

    public void Update(Priority priority, string? justification, IEnumerable<RequisitionLine> lines)
    {
        Priority = priority;
        Justification = justification;
        Lines = [.. lines];
    }
}