using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using Purchasely.Application.DTOs;
using Purchasely.Application.Features.Products.Commands;
using Purchasely.Application.Features.Products.Queries;
using Purchasely.Application.Features.Users.Commands;

namespace Purchasely.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(
    IMediator mediator,
    IWebHostEnvironment environment
    ) : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result.Errors);

        return Created("", result.Value);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result.Errors);

        AppendCookies(result.Value!.Tokens.AccessToken, result.Value!.Tokens.RefreshToken);

        return Ok(result.Value!.User);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies["refresh"];
        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized("Refresh token not found");

        var result = await mediator.Send(new RefreshTokenCommand(refreshToken), cancellationToken);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result.Errors);

        AppendCookies(result.Value!.AccessToken, result.Value!.RefreshToken);

        return NoContent();
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies["refresh"];
        if (!string.IsNullOrEmpty(refreshToken))
        {
            var result = await mediator.Send(new LogoutCommand(refreshToken), cancellationToken);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.Errors);
        }

        Response.Cookies.Delete("auth");
        Response.Cookies.Delete("refresh");

        return NoContent();
    }

    private void AppendCookies(string accessToken, string refreshToken)
    {
        var accessTokenOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = !environment.IsDevelopment(),
            SameSite = environment.IsDevelopment() ? SameSiteMode.Lax : SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddMinutes(15)
        };

        Response.Cookies.Append("auth", accessToken, accessTokenOptions);

        var refreshTokenOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = !environment.IsDevelopment(),
            SameSite = environment.IsDevelopment() ? SameSiteMode.Lax : SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        };

        Response.Cookies.Append("refresh", refreshToken, refreshTokenOptions);
    }
}