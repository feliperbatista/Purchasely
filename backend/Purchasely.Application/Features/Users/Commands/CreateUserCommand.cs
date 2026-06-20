using AutoMapper;
using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;
using Purchasely.Domain.Enums;

namespace Purchasely.Application.Features.Users.Commands;

public record CreateUserCommand(
    string Name,
    string Email,
    string Password,
    UserRole Role
) : IRequest<Result<UserResponse>>;

public class CreateUserCommandHandler(
    IUserRepository repository,
    IMapper mapper
) : IRequestHandler<CreateUserCommand, Result<UserResponse>>
{
    public async Task<Result<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await repository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingUser is not null)
            return Result<UserResponse>.Failure(409, "Email already registerd");

        var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = User.Create(request.Name, request.Email, hash, request.Role);
        
        await repository.AddAsync(user, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return Result<UserResponse>.Success(mapper.Map<UserResponse>(user));
    }
}