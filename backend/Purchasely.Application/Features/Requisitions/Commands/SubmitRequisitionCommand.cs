using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.Events.Requisitions;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Enums;

namespace Purchasely.Application.Features.Requisitions.Commands;

public record SubmitRequisitionCommand(
    Guid Id
) : IRequest<Result<Unit>>;

public class SubmitRequisitionCommandHandler(
    IRequisitionRepository requisitionRepo,
    IMediator mediator,
    ICurrentUserService currentUser,
    IUserRepository userRepository
) : IRequestHandler<SubmitRequisitionCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(SubmitRequisitionCommand request, CancellationToken cancellationToken)
    {
        var requisition = await requisitionRepo.GetByIdAsync(request.Id, cancellationToken);
        if (requisition is null)
            return Result<Unit>.Failure(404, "Requisition not found");

        if (!requisition.CanTransitionTo(RequisitionStatus.Submitted))
        {
            return Result<Unit>.Failure(
                400,
                $"Cannot change status from {requisition.Status} to {RequisitionStatus.Submitted}"
            );
        }

        requisition.Submit();
        await requisitionRepo.SaveChangesAsync(cancellationToken);

        var approvers = await userRepository.GetByRoleAsync(UserRole.Manager, cancellationToken);
        var approversEmail = approvers.Select(a => a.Email).ToList();
        var approversId = approvers.Select(a => a.Id).ToList();

        await mediator.Publish(new RequisitionSubmittedEvent(
            request.Id,
            currentUser.Id,
            DateTime.UtcNow,
            currentUser.Name,
            requisition.Number,
            approversEmail,
            approversId
        ), cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}