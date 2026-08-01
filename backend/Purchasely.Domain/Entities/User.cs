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
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiresAt { get; set; }

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

    public void SetRefreshToken(string token, int expiryDays = 7)
    {
        RefreshToken = token;
        RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(expiryDays);
    }

    public void RevokeRefreshToken()
    {
        RefreshToken = null;
        RefreshTokenExpiresAt = null;
    }

    public bool IsRefreshTokenActive() =>
        RefreshToken is not null &&
        RefreshTokenExpiresAt.HasValue &&
        DateTime.UtcNow < RefreshTokenExpiresAt;

    public void ChangeRole(UserRole role) => Role = role;
}