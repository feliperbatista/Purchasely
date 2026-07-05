namespace Purchasely.Application.Interfaces;

public record NotificationPayload(
    string Title,
    string Message,
    string Type,
    Guid? EntityId,
    string? EntityType
);

public interface INotificationService
{
    Task SendToUserAsync(Guid userId, NotificationPayload payload, CancellationToken cancellationToken);
    Task SendToUsersAsync(IEnumerable<Guid> usersId, NotificationPayload payload, CancellationToken cancellationToken);
}