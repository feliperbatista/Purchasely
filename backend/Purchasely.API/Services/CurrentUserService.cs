using System.Security.Claims;
using Purchasely.Application.Interfaces;

namespace Purchasely.API.Services;

public class CurrentUserService(IHttpContextAccessor accessor) : ICurrentUserService
{
    public Guid Id => Guid.Parse(accessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public string Name => accessor.HttpContext!.User.FindFirstValue(ClaimTypes.Name)!;

    public string Email => accessor.HttpContext!.User.FindFirstValue(ClaimTypes.Email)!;
}