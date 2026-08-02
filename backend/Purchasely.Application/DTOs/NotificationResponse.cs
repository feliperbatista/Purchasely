namespace Purchasely.Application.DTOs;

public record NotificationResponse(
    Guid Id,
    string Title,
    string Message,
    string Type, 
    string EntityId,
    string EntityType,
    bool Read,
    DateTime CreatedAt
);