using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.Events.Requisitions;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Enums;

namespace Purchasely.Application.Features.Requisitions.Commands;

public record RemoveApprovalRequisitionCommand(
    Guid Id
) : IRequest<Result<Unit>>;

public class RemoveApprovalRequisitionCommandHandler(
    IRequisitionRepository requisitionRepo,
    IMediator mediator,
    ICurrentUserService currentUser
) : IRequestHandler<RemoveApprovalRequisitionCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(RemoveApprovalRequisitionCommand request, CancellationToken cancellationToken)
    {
        var requisition = await requisitionRepo.GetByIdAsync(request.Id, cancellationToken);
        if (requisition is null)
            return Result<Unit>.Failure(404, "Requisition not found");

        var approval = requisition.Approvals.FirstOrDefault(a => a.ApproverId == currentUser.Id);

        if (approval is null)
            return Result<Unit>.Failure(400, $"You did not approve this requisition");

        requisition.RemoveApproval(approval);
        var saved = await requisitionRepo.SaveChangesAsync(cancellationToken);

        if (!saved)
            return Result<Unit>.Failure(400, "Failed saving in database");

        await mediator.Publish(new RequisitionApprovalRemovedEvent(
            request.Id,
            currentUser.Id,
            DateTime.UtcNow
        ), cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}