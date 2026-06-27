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
    ICurrentUserService currentUser
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
        var saved = await requisitionRepo.SaveChangesAsync(cancellationToken);

        if (!saved)
            return Result<Unit>.Failure(400, "Failed saving in database");

        await mediator.Publish(new RequisitionSubmittedEvent(
            request.Id,
            currentUser.Id,
            DateTime.UtcNow
        ), cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}