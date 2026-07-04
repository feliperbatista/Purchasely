using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.Events.PurchaseOrders;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Enums;

namespace Purchasely.Application.Features.PurchaseOrders.Commands;

public record ClosePurchaseOrderCommand(
    Guid Id
) : IRequest<Result<Unit>>;

public class ClosePurchaseOrderCommandHandler(
    IPurchaseOrderRepository purchaseOrderRepo,
    IMediator mediator,
    ICurrentUserService currentUser
) : IRequestHandler<ClosePurchaseOrderCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(ClosePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var purchaseOrder = await purchaseOrderRepo.GetByIdAsync(request.Id, cancellationToken);
        if (purchaseOrder is null)
            return Result<Unit>.Failure(404, "Purchase order not found");

        if (!purchaseOrder.CanTransitionTo(PurchaseOrderStatus.Closed))
        {
            return Result<Unit>.Failure(
                400,
                $"Cannot change status from {purchaseOrder.Status} to {PurchaseOrderStatus.Closed}"
            );
        }

        purchaseOrder.Close();
        
        var saved = await purchaseOrderRepo.SaveChangesAsync(cancellationToken);

        if (!saved)
            return Result<Unit>.Failure(400, "Failed saving in database");

        await mediator.Publish(new PurchaseOrderClosedEvent(
            request.Id,
            currentUser.Id,
            DateTime.UtcNow
        ), cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
};