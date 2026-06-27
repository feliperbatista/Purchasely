namespace Purchasely.Domain.Entities;

public class Notification
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public required string Title { get; set; }
    public required string Message { get; set; }
    public bool Read { get; set; }
    public DateTime CreatedAt { get; set; }

    private Notification() {}

    public static Notification Create(Guid userId, string title, string message)
    {
        return new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = title,
            Message = message,
            Read = false,
            CreatedAt = DateTime.UtcNow
        };
    }
}