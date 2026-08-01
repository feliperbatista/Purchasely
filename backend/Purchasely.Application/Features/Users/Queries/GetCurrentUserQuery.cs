using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Enums;

namespace Purchasely.Application.Features.Users.Queries;

public record GetCurrentUserQuery() : IRequest<Result<UserResponse>>;

public class GetCurrentUserQueryHandler(
    ICurrentUserService currentUser
) : IRequestHandler<GetCurrentUserQuery, Result<UserResponse>>
{
    public async Task<Result<UserResponse>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        return Result<UserResponse>.Success(new UserResponse
        (
            currentUser.Id,
            currentUser.Name,
            currentUser.Email,
            Enum.Parse<UserRole>(currentUser.Role),
            null
        ));
    }
}