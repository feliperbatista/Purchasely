using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;

namespace Purchasely.Application.Features.PurchaseOrders.Queries;

public record GetPurchaseOrderByIdQuery(Guid Id) : IRequest<Result<PurchaseOrderResponse>>;

public class GetPurchaseOrderByIdQueryHandler(
    IPurchaseOrderRepository repository
) : IRequestHandler<GetPurchaseOrderByIdQuery, Result<PurchaseOrderResponse>>
{
    public async Task<Result<PurchaseOrderResponse>> Handle(GetPurchaseOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var po = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (po is null)
            return Result<PurchaseOrderResponse>.Failure(404, "Purchase order not found");

        return Result<PurchaseOrderResponse>.Success(new PurchaseOrderResponse(
                po.Id,
                po.Number,
                po.SupplierId,
                po.Supplier.Name,
                po.Creator.Name,
                po.Status,
                po.SubTotal,
                po.TaxAmount,
                po.TotalAmount,
                po.CancellationReason,
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
                [.. po.Documents.Select(d => new PurchaseOrderDocumentsResponse(
                    d.Id,
                    d.FileName,
                    d.ContentType,
                    d.BlobUrl
                ))]
            ));
    }
}