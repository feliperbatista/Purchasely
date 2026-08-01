using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;

namespace Purchasely.Application.Features.Users.Queries;

public record GetUsersQuery(
    int Page = 1,
    int PageSize = 20
) : IRequest<Result<PaginatedResponse<UserResponse>>>;

public class GetUsersQueryHandler(
    IUserRepository repository
) : IRequestHandler<GetUsersQuery, Result<PaginatedResponse<UserResponse>>>
{
    public async Task<Result<PaginatedResponse<UserResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await repository.GetAllAsync(request.Page, request.PageSize, cancellationToken);
        var totalCount = await repository.CountAsync(cancellationToken);

        return Result<PaginatedResponse<UserResponse>>.Success(new PaginatedResponse<UserResponse>(
            Items: [.. users.Select(u => new UserResponse(u.Id, u.Name, u.Email, u.Role, u.CreatedAt))],
            CurrentPage: request.Page,
            PageSize: request.PageSize,
            TotalCount: totalCount
        ));
    }
}