namespace Purchasely.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public required string SKU { get; set; }
    public required string Name { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Description { get; set; }
    public Supplier Supplier { get; set; } = null!;
    public Guid SupplierId { get; set; }
    public DateTime CreatedAt { get; set; }

    private Product() {}

    public static Product Create(string sku, string name, decimal unitPrice, string? description, Guid supplierId)
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            SKU = sku,
            Name = name,
            UnitPrice = unitPrice,
            Description = description,
            SupplierId = supplierId,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(string sku, string name, decimal unitPrice, string? description)
    {
        SKU = sku;
        Name = name;
        UnitPrice = unitPrice;
        Description = description;
    }
}