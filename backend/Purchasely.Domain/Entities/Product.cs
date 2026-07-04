namespace Purchasely.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public required string SKU { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public DateTime CreatedAt { get; set; }

    private Product() {}

    public static Product Create(string sku, string name, string? description, string? category)
    {
        return new Product
        {
            SKU = sku,
            Name = name,
            Description = description,
            Category = category,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(string sku, string name, string? description, string? category)
    {
        SKU = sku;
        Name = name;
        Description = description;
        Category = category;
    }
}