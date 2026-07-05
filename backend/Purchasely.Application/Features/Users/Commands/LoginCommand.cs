using System.Security.Cryptography;
using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;

namespace Purchasely.Application.Features.Users.Commands;

public record LoginCommand(
    string Email,
    string Password
) : IRequest<Result<LoginResponse>>;

public class LoginCommandHandler(
    IUserRepository repository,
    IJwtService jwtService) : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await repository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null)
            return Result<LoginResponse>.Failure(401, "Invalid credentials");

        var valid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if (!valid)
            return Result<LoginResponse>.Failure(401, "Invalid credentials");

        var token = jwtService.GenerateToken(user);
        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        user.SetRefreshToken(refreshToken);
        await repository.SaveChangesAsync(cancellationToken);

        return Result<LoginResponse>.Success(new LoginResponse(new TokensResponse(token, refreshToken), new UserResponse(user.Id, user.Name, user.Email, user.Role)));
    }
}