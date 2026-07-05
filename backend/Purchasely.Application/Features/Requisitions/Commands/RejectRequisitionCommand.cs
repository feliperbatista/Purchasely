using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.Events.Requisitions;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Enums;

namespace Purchasely.Application.Features.Requisitions.Commands;

public record RejectRequisitionCommand(
    Guid Id,
    string Reason
) : IRequest<Result<Unit>>;

public class RejectRequisitionCommandHandler(
    IRequisitionRepository requisitionRepo,
    IMediator mediator,
    ICurrentUserService currentUser
) : IRequestHandler<RejectRequisitionCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(RejectRequisitionCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Reason))
            return Result<Unit>.Failure(400, "Reason is required");

        var requisition = await requisitionRepo.GetByIdAsync(request.Id, cancellationToken);
        if (requisition is null)
            return Result<Unit>.Failure(404, "Requisition not found");

        if (!requisition.CanTransitionTo(RequisitionStatus.Rejected))
        {
            return Result<Unit>.Failure(
                400,
                $"Cannot change status from {requisition.Status} to {RequisitionStatus.Rejected}"
            );
        }

        requisition.Reject();
        var saved = await requisitionRepo.SaveChangesAsync(cancellationToken);

        if (!saved)
            return Result<Unit>.Failure(400, "Failed saving in database");

        await mediator.Publish(new RequisitionRejectedEvent(
            request.Id,
            requisition.RequesterId,
            currentUser.Id,
            request.Reason,
            DateTime.UtcNow,
            requisition.Requester.Email,
            requisition.Requester.Name,
            requisition.Number
        ), cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}