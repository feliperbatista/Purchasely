using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;

namespace Purchasely.Application.Features.PurchaseOrders.Queries;

public record GetPurchaseOrdersQuery : IRequest<Result<List<PurchaseOrderResponse>>>;

public class GetPurchaseOrdersQueryHandler(
    IPurchaseOrderRepository repository
) : IRequestHandler<GetPurchaseOrdersQuery, Result<List<PurchaseOrderResponse>>>
{
    public async Task<Result<List<PurchaseOrderResponse>>> Handle(GetPurchaseOrdersQuery request, CancellationToken cancellationToken)
    {
        var purchaseOrders = await repository.GetAllAsync(cancellationToken);
        return Result<List<PurchaseOrderResponse>>.Success([.. purchaseOrders.Select(po => new PurchaseOrderResponse(
                    po.Id,
                    po.Number,
                    po.SupplierId,
                    po.Supplier.Name,
                    null,
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
                    ))]
                ))]);
    }
}