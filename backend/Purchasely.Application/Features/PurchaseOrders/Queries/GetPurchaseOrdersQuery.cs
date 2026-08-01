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
    DateTime? To = null,
    Guid? SupplierId = null
) : IRequest<Result<PaginatedResponse<PurchaseOrderResponse>>>;

public class GetPurchaseOrdersQueryHandler(
    IPurchaseOrderRepository purchaseOrderRepo,
    ISupplierRepository supplierRepo
) : IRequestHandler<GetPurchaseOrdersQuery, Result<PaginatedResponse<PurchaseOrderResponse>>>
{
    public async Task<Result<PaginatedResponse<PurchaseOrderResponse>>> Handle(GetPurchaseOrdersQuery request, CancellationToken cancellationToken)
    {
        if (request.SupplierId is not null)
        {
            var supplier = await supplierRepo.GetByIdAsync(request.SupplierId.Value, cancellationToken);
            if (supplier is null)
                return Result<PaginatedResponse<PurchaseOrderResponse>>.Failure(404, "Supplier not found");
        }

        var purchaseOrders = await purchaseOrderRepo.GetAllAsync(
            request.Page,
            request.PageSize,
            request.Status,
            request.From,
            request.To,
            request.SupplierId,
            cancellationToken);
        
        var poCount = await purchaseOrderRepo.CountAsync(cancellationToken);
        
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
                    null,
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