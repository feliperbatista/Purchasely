namespace Purchasely.Domain.Entities;

public class AuditLog
{
    public Guid Id { get; set; }
    public required string EntityType { get; set; }
    public Guid EntityId { get; set; }
    public required string Action { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    private AuditLog() {}

    public static AuditLog Create(string entityType, Guid entityId, string action, Guid userId, DateTime createdAt)
    {
        return new AuditLog
        {
            EntityType = entityType,
            EntityId = entityId,
            Action = action,
            UserId = userId,
            CreatedAt = createdAt
        };
    }
}