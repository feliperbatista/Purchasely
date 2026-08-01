using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Enums;

namespace Purchasely.Application.Features.Users.Commands;

public record ChangeRoleCommand(
    Guid Id,
    UserRole Role
) : IRequest<Result<Unit>>;

public class ChangeRoleCommandHandler(IUserRepository repository) : IRequestHandler<ChangeRoleCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(ChangeRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (user is null)
            return Result<Unit>.Failure(404, "User not found");

        user.ChangeRole(request.Role);

        await repository.SaveChangesAsync(cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}