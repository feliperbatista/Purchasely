namespace Purchasely.Domain.Entities;

public class PurchaseOrderLine
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public Guid PurchaseOrderId { get; set; }
    public PurchaseOrder PurchaseOrder { get; set; } = null!;
    public decimal QuantityOrdered { get; set; }
    public decimal QuantityReceived { get; set; }
    public decimal UnitPrice { get; set; }

    private PurchaseOrderLine() {}

    public static PurchaseOrderLine Create(Guid productId, decimal quantityOrdered, decimal unitPrice)
    {
        return new PurchaseOrderLine
        {
            ProductId = productId,
            QuantityOrdered = quantityOrdered,
            UnitPrice = unitPrice
        };
    }

    public void Receive(decimal quantity)
    {
        QuantityReceived += quantity;
    }
}