using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.Interfaces;

namespace Purchasely.Application.Features.Notifications.Commands;

public record MarkNotificationAsReadCommand(Guid Id) : IRequest<Result<Unit>>;

public class MarkNotificationAsReadCommandHandler(
    INotificationRepository notificationRepo,
    ICurrentUserService currentUser
) : IRequestHandler<MarkNotificationAsReadCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(
        MarkNotificationAsReadCommand request,
        CancellationToken cancellationToken)
    {
        var notification = await notificationRepo.GetByIdAsync(request.Id, cancellationToken);

        if (notification is null)
            return Result<Unit>.Failure(404, "Notification not found");

        if (notification.UserId != currentUser.Id)
            return Result<Unit>.Failure(403, "Forbidden");

        await notificationRepo.MarkAsReadAsync(request.Id, cancellationToken);
        await notificationRepo.SaveChangesAsync(cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}