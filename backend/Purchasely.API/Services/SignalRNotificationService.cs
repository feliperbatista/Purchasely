using Microsoft.AspNetCore.SignalR;
using Purchasely.API.Hubs;
using Purchasely.Application.Interfaces;

namespace Purchasely.API.Services;

public class SignalRNotificationService(IHubContext<NotificationHub> hubContext)
    : INotificationService
{
    public async Task SendToUserAsync(Guid userId, NotificationPayload payload, CancellationToken cancellationToken)
    {
        await hubContext.Clients
            .Group(userId.ToString())
            .SendAsync("ReceiveNotification", payload, cancellationToken);
    }

    public async Task SendToUsersAsync(IEnumerable<Guid> usersId, NotificationPayload payload, CancellationToken cancellationToken)
    {
        var groups = usersId.Select(id => id.ToString()).ToList();

        await hubContext.Clients
            .Groups(groups)
            .SendAsync("ReceiveNotification", payload, cancellationToken);
    }
}