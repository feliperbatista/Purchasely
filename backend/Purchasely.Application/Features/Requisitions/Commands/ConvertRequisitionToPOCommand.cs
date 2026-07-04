using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Events.Requisitions;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;

namespace Purchasely.Application.Features.Requisitions.Commands;

public record CreatePOLineCommand(
    Guid RequisitionLineId,
    Guid SupplierId,
    decimal UnitPrice
);

public record ConvertRequisitionToPOCommand(
    Guid RequisitionId,
    List<CreatePOLineCommand> Lines
) : IRequest<Result<CreatePurchaseOrderResponse>>;

public class ConvertRequisitionToPOCommandHandler(
    IRequisitionRepository requisitionRepo,
    ISupplierRepository supplierRepo,
    IPurchaseOrderRepository purchaseOrderRepo,
    ICurrentUserService currentUser,
    IMediator mediator
) : IRequestHandler<ConvertRequisitionToPOCommand, Result<CreatePurchaseOrderResponse>>
{
    public async Task<Result<CreatePurchaseOrderResponse>> Handle(ConvertRequisitionToPOCommand request, CancellationToken cancellationToken)
    {
        var requisition = await requisitionRepo.GetByIdAsync(request.RequisitionId, cancellationToken);
        if (requisition is null)
        return Result<CreatePurchaseOrderResponse>.Failure(404, "Requisition not found.");

        if (!requisition.CanTransitionTo(Domain.Enums.RequisitionStatus.ConvertedToPO))
            return Result<CreatePurchaseOrderResponse>.Failure(400, $"Requisition is not approved.");

        var coveredLineIds = request.Lines.Select(l => l.RequisitionLineId).ToHashSet();
        var requisitionLineIds = requisition.Lines.Select(l => l.Id).ToHashSet();
        if (!requisitionLineIds.SetEquals(coveredLineIds))
            return Result<CreatePurchaseOrderResponse>.Failure(400, "All requisition lines must be mapped to a PO line.");

        var supplierIds = request.Lines.Select(l => l.SupplierId).Distinct().ToList();
        var suppliers = await supplierRepo.GetByIdsAsync(supplierIds, cancellationToken);

        if (suppliers.Count != supplierIds.Count)
            return Result<CreatePurchaseOrderResponse>.Failure(400, "One or more suppliers not found.");

        var linesBySupplier = request.Lines.GroupBy(l => l.SupplierId);
        var purchaseOrders = new List<PurchaseOrder>();

        foreach(var group in linesBySupplier)
        {
            var supplierId = group.Key;
            var poLines = group.Select(l =>
            {
                var reqLine = requisition.Lines.First(rl => rl.Id == l.RequisitionLineId);
                return PurchaseOrderLine.Create(
                    reqLine.ProductId,
                    reqLine.QuantityRequested,
                    l.UnitPrice
                );
            })
            .ToList();

            var po = PurchaseOrder.Create(
                supplierId,
                request.RequisitionId,
                currentUser.Id,
                poLines
            );

            purchaseOrders.Add(po);
            await purchaseOrderRepo.AddAsync(po, cancellationToken);
        }

        requisition.ConvertToPO();
        
        var saved = await requisitionRepo.SaveChangesAsync(cancellationToken);

        if (!saved)
            return Result<CreatePurchaseOrderResponse>.Failure(400, "Failed saving in database");

        await mediator.Publish(new RequisitionConvertedToPOEvent(
            requisition.Id,
            currentUser.Id,
            DateTime.UtcNow,
            [.. purchaseOrders.Select(p => p.Id)]
        ), cancellationToken);

        return Result<CreatePurchaseOrderResponse>.Success(new CreatePurchaseOrderResponse(
                request.RequisitionId,
                [.. purchaseOrders.Select(po => new PurchaseOrderResponse(
                    po.Id,
                    po.Number,
                    po.SupplierId,
                    suppliers.First(s => s.Id == po.SupplierId).Name,
                    currentUser.Name,
                    po.Status,
                    po.SubTotal,
                    po.TaxAmount,
                    po.TotalAmount,
                    po.CreatedAt,
                    po.IssuedAt,
                    [.. po.Lines.Select(l => new PurchaseOrderLineResponse(
                        l.Id,
                        l.ProductId,
                        l.Product.Name,
                        l.QuantityOrdered,
                        l.QuantityReceived,
                        l.UnitPrice,
                        l.QuantityOrdered * l.UnitPrice
                    ))],
                    null
                ))]
            ));
    }
}