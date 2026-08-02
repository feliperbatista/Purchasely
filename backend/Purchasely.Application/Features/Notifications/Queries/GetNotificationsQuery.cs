using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.Interfaces;

namespace Purchasely.Application.Features.Notifications.Queries;

public record GetNotificationsQuery : IRequest<Result<List<NotificationResponse>>>;

public record NotificationResponse(
    Guid Id,
    string Title,
    string Message,
    string Type,
    Guid? EntityId,
    string? EntityType,
    bool Read,
    DateTime CreatedAt
);

public class GetNotificationsQueryHandler(
    INotificationRepository notificationRepo,
    ICurrentUserService currentUser
) : IRequestHandler<GetNotificationsQuery, Result<List<NotificationResponse>>>
{
    public async Task<Result<List<NotificationResponse>>> Handle(
        GetNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        var notifications = await notificationRepo.GetByUserIdAsync(currentUser.Id, cancellationToken);

        return Result<List<NotificationResponse>>.Success(
            [.. notifications.Select(n => new NotificationResponse(
                n.Id, n.Title, n.Message, n.Type,
                n.EntityId, n.EntityType, n.Read, n.CreatedAt
            ))]);
    }
}