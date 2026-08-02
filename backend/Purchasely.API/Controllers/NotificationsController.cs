using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Purchasely.Application.Features.Notifications.Commands;
using Purchasely.Application.Features.Notifications.Queries;

namespace Purchasely.API.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationsController(ISender mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetNotificationsQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : StatusCode(result.StatusCode, result.Errors);
    }

    [HttpPatch("{id:guid}/read")]
    public async Task<IActionResult> MarkAsRead([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new MarkNotificationAsReadCommand(id), cancellationToken);
        return result.IsSuccess ? NoContent() : StatusCode(result.StatusCode, result.Errors);
    }

    [HttpPatch("read-all")]
    public async Task<IActionResult> MarkAllAsRead(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new MarkAllNotificationsAsReadCommand(), cancellationToken);
        return result.IsSuccess ? NoContent() : StatusCode(result.StatusCode, result.Errors);
    }
}