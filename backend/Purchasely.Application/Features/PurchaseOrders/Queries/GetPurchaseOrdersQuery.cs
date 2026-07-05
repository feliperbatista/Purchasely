using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Enums;

namespace Purchasely.Application.Features.PurchaseOrders.Queries;

public record GetPurchaseOrdersQuery(
    int Page = 1,
    int PageSize = 20,
    PurchaseOrderStatus? Status = null,
    DateTime? From = null,
    DateTime? To = null
) : IRequest<Result<PaginatedResponse<PurchaseOrderResponse>>>;

public class GetPurchaseOrdersQueryHandler(
    IPurchaseOrderRepository repository
) : IRequestHandler<GetPurchaseOrdersQuery, Result<PaginatedResponse<PurchaseOrderResponse>>>
{
    public async Task<Result<PaginatedResponse<PurchaseOrderResponse>>> Handle(GetPurchaseOrdersQuery request, CancellationToken cancellationToken)
    {
        var purchaseOrders = await repository.GetAllAsync(
            request.Page,
            request.PageSize,
            request.Status,
            request.From,
            request.To,
            cancellationToken);
        
        var poCount = await repository.CountAsync(cancellationToken);
        
        return Result<PaginatedResponse<PurchaseOrderResponse>>.Success(new PaginatedResponse<PurchaseOrderResponse>(
            Items: [.. purchaseOrders.Select(po => new PurchaseOrderResponse(
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
                    ))],
                    null
                ))],
                request.Page,
                request.PageSize,
                poCount
        )
        );
    }
}