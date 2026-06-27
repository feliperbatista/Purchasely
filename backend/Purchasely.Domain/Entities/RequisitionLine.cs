namespace Purchasely.Domain.Entities;

public class RequisitionLine
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public Guid RequisitionId { get; set; }
    public Requisition Requisition { get; set; } = null!;
    public decimal QuantityRequested { get; set; }
    public decimal EstimatedUnitPrice { get; set; }

    private RequisitionLine() {}

    public static RequisitionLine Create(Guid productId, decimal quantityRequested, decimal estimatedUnitPrice)
    {
        return new RequisitionLine
        {
            Id = Guid.NewGuid(),
            ProductId = productId,
            QuantityRequested = quantityRequested,
            EstimatedUnitPrice = estimatedUnitPrice
        };
    }
}