using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;

namespace Purchasely.Application.Features.Users.Queries;

public record HasUsersQuery() : IRequest<Result<bool>>;

public class HasUsersQueryHandler(
    IUserRepository repository
) : IRequestHandler<HasUsersQuery, Result<bool>>
{
    public async Task<Result<bool>> Handle(HasUsersQuery request, CancellationToken cancellationToken)
    {
        var usersCount = await repository.CountAsync(cancellationToken);

        return usersCount > 0 ? Result<bool>.Success(true) : Result<bool>.Success(false);
    }
}