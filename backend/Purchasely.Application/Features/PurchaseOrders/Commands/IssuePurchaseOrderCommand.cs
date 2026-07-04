using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.Events.PurchaseOrders;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Enums;

namespace Purchasely.Application.Features.PurchaseOrders.Commands;

public record IssuePurchaseOrderCommand(
    Guid Id
) : IRequest<Result<Unit>>;

public class IssuePurchaseOrderCommandHandler(
    IPurchaseOrderRepository purchaseOrderRepo,
    IMediator mediator,
    ICurrentUserService currentUser
) : IRequestHandler<IssuePurchaseOrderCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(IssuePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var purchaseOrder = await purchaseOrderRepo.GetByIdAsync(request.Id, cancellationToken);
        if (purchaseOrder is null)
            return Result<Unit>.Failure(404, "Purchase order not found");

        if (!purchaseOrder.CanTransitionTo(PurchaseOrderStatus.Issued))
        {
            return Result<Unit>.Failure(
                400,
                $"Cannot change status from {purchaseOrder.Status} to {PurchaseOrderStatus.Issued}"
            );
        }

        purchaseOrder.Issue();
        
        var saved = await purchaseOrderRepo.SaveChangesAsync(cancellationToken);

        if (!saved)
            return Result<Unit>.Failure(400, "Failed saving in database");

        await mediator.Publish(new PurchaseOrderIssuedEvent(
            request.Id,
            currentUser.Id,
            DateTime.UtcNow
        ), cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
};