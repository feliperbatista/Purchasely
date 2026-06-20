namespace Purchasely.Domain.Entities;

public class Supplier
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public required string TaxNumber { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    private Supplier() {}

    public static Supplier Create(string name, string email, string phone, string address, string taxNumber)
    {
        return new Supplier
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email,
            Phone = phone,
            Address = address,
            TaxNumber = taxNumber,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(string name, string email, string phone, string address, string taxNumber)
    {
        Name = name;
        Email = email;
        Phone = phone;
        Address = address;
        TaxNumber = taxNumber;
    }

    public void Disable()
    {
        IsActive = false;
    }
}