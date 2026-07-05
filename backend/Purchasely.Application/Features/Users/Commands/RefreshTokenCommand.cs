using System.Security.Cryptography;
using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;

namespace Purchasely.Application.Features.Users.Commands;

public record RefreshTokenCommand(
    string RefreshToken
) : IRequest<Result<TokensResponse>>;

public class RefreshTokenCommandHandler(
    IUserRepository repository,
    IJwtService jwtService) : IRequestHandler<RefreshTokenCommand, Result<TokensResponse>>
{
    public async Task<Result<TokensResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await repository.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);

        if (user is null || !user.IsRefreshTokenActive())
            return Result<TokensResponse>.Failure(401, "Invalid or expired refresh token");

        var newToken = jwtService.GenerateToken(user);
        var newRefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        user.SetRefreshToken(newRefreshToken);
        await repository.SaveChangesAsync(cancellationToken);

        return Result<TokensResponse>.Success(new TokensResponse(newToken, newRefreshToken));
    }
}