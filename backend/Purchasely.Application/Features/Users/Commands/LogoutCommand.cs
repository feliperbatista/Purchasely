using System.Security.Cryptography;
using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;

namespace Purchasely.Application.Features.Users.Commands;

public record LogoutCommand(
    string RefreshToken
) : IRequest<Result<Unit>>;

public class LogoutCommandHandler(
    IUserRepository repository) : IRequestHandler<LogoutCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var user = await repository.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);

        if (user is not null)
        {
            user.RevokeRefreshToken();
            await repository.SaveChangesAsync(cancellationToken);
        }

        return Result<Unit>.Success(Unit.Value);
    }
}