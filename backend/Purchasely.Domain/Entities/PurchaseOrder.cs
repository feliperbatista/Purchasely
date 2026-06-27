using Purchasely.Domain.Enums;

namespace Purchasely.Domain.Entities;

public class PurchaseOrder
{
    public Guid Id { get; set; }
    public int Number { get; set; }
    public Guid SupplierId { get; set;}
    public Supplier Supplier { get; set; } = null!;
    public Guid RequisitionId { get; set;}
    public Requisition Requisition { get; set; } = null!;
    public Guid CreatedBy { get; set;}
    public User Creator { get; set; } = null!;
    public decimal SubTotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public PurchaseOrderStatus Status { get; set; } = PurchaseOrderStatus.Draft;
    public DateTime CreatedAt { get; set; }
    public DateTime? IssuedAt { get; set; }
    public ICollection<PurchaseOrderLine> Lines { get; set; } = [];

    private PurchaseOrder() {}

    public static PurchaseOrder Create(
        Guid supplierId,
        Guid requisitionId,
        Guid createdBy,
        List<PurchaseOrderLine> lines,
        decimal taxRate = 0)
    {
        var subTotal = lines.Sum(l => l.QuantityOrdered * l.UnitPrice);
        var taxAmount = subTotal * taxRate;

        return new PurchaseOrder
        {
            Id = Guid.NewGuid(),
            SupplierId = supplierId,
            RequisitionId = requisitionId,
            CreatedBy = createdBy,
            SubTotal = subTotal,
            TaxAmount = taxAmount,
            TotalAmount = subTotal + taxAmount,
            Status = PurchaseOrderStatus.Draft,
            CreatedAt = DateTime.UtcNow,
            Lines = lines
        };
    }

    public void Issue()
    {
        Status = PurchaseOrderStatus.Issued;
        IssuedAt = DateTime.UtcNow;
    }

    public void PartiallyReceive()
    {
        Status = PurchaseOrderStatus.PartiallyReceived;
    }

    public void Receive()
    {
        Status = PurchaseOrderStatus.Received;
    }

    public void Close()
    {
        Status = PurchaseOrderStatus.Closed;
    }

    public void Cancel()
    {
        Status = PurchaseOrderStatus.Cancelled;
    }

    public bool CanTransitionTo(PurchaseOrderStatus newStatus)
    {
        return Status switch
        {
            PurchaseOrderStatus.Draft =>
                newStatus == PurchaseOrderStatus.Issued ||
                newStatus == PurchaseOrderStatus.Cancelled,

            PurchaseOrderStatus.Issued =>
                newStatus == PurchaseOrderStatus.PartiallyReceived ||
                newStatus == PurchaseOrderStatus.Received ||
                newStatus == PurchaseOrderStatus.Cancelled,

            PurchaseOrderStatus.PartiallyReceived =>
                newStatus == PurchaseOrderStatus.Received,

            PurchaseOrderStatus.Received =>
                newStatus == PurchaseOrderStatus.Closed,

            PurchaseOrderStatus.Closed => false,
            PurchaseOrderStatus.Cancelled => false,

            _ => false
        };
    }
}