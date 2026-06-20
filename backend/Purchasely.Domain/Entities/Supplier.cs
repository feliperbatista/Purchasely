namespace Purchasely.Domain.Entities;

public class Supplier
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required string Address { get; set; }
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
            TaxNumber = NormalizeTaxNumber(taxNumber),
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
        TaxNumber = NormalizeTaxNumber(taxNumber);
    }

    public void Disable()
    {
        IsActive = false;
    }

    private static string NormalizeTaxNumber(string taxNumber)
    {
        return new string([.. taxNumber.Where(char.IsDigit)]);
    }
}