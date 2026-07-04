namespace Purchasely.Domain.Entities;

public class SupplierProduct
{
    public Guid Id { get; set; }
    public Guid SupplierId { get; set; }
    public Supplier Supplier { get; set; } = null!;
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public decimal UnitPrice { get; set; }

    private SupplierProduct() {}

    public static SupplierProduct Create(Guid supplierId, Guid productId, decimal unitPrice)
    {
        return new SupplierProduct
        {
            SupplierId = supplierId,
            ProductId = productId,
            UnitPrice = unitPrice
        };
    }
}