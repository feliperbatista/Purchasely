namespace Purchasely.Domain.Entities;

public class Notification
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public required string Title { get; set; }
    public required string Message { get; set; }
    public required string Type { get; set; }
    public Guid? EntityId { get; set; }
    public string? EntityType { get; set; }
    public bool Read { get; set; }
    public DateTime CreatedAt { get; set; }

    private Notification() {}

    public static Notification Create(Guid userId, string title, string message, string type, Guid? entityId, string? entityType)
    {
        return new Notification
        {
            UserId = userId,
            Title = title,
            Message = message,
            Read = false,
            CreatedAt = DateTime.UtcNow,
            Type = type,
            EntityId = entityId,
            EntityType = entityType
        };
    }

    public void MarkAsRead() => Read = true;
}