using Purchasely.Domain.Enums;

namespace Purchasely.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<Requisition> Requisitions = [];

    private User() {}

    public static User Create(string name, string email, string passwordHash, UserRole role)
    {
        return new User
        {
            Name = name,
            Email = email,
            PasswordHash = passwordHash,
            Role = role,
            CreatedAt = DateTime.UtcNow
        };
    }
}