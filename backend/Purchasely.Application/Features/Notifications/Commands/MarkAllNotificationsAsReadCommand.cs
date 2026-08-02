using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.Interfaces;

namespace Purchasely.Application.Features.Notifications.Commands;

public record MarkAllNotificationsAsReadCommand : IRequest<Result<Unit>>;

public class MarkAllNotificationsAsReadCommandHandler(
    INotificationRepository notificationRepo,
    ICurrentUserService currentUser
) : IRequestHandler<MarkAllNotificationsAsReadCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(
        MarkAllNotificationsAsReadCommand request,
        CancellationToken cancellationToken)
    {
        await notificationRepo.MarkAllAsReadAsync(currentUser.Id, cancellationToken);
        return Result<Unit>.Success(Unit.Value);
    }
}