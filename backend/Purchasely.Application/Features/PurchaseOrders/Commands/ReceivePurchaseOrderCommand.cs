using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Events.PurchaseOrders;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Enums;

namespace Purchasely.Application.Features.PurchaseOrders.Commands;

public record PurchaseOrderLinesReceived(
    Guid Id,
    decimal Quantity
);

public record ReceivePurchaseOrderCommand(
    Guid Id,
    List<PurchaseOrderLinesReceived> Lines,
    List<ReceiptFileDto> Files
) : IRequest<Result<Unit>>;

public class ReceivePurchaseOrderCommandHandler(
    IPurchaseOrderRepository purchaseOrderRepo,
    IMediator mediator,
    ICurrentUserService currentUser,
    IFileStorageService fileStorage
) : IRequestHandler<ReceivePurchaseOrderCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(ReceivePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var purchaseOrder = await purchaseOrderRepo.GetByIdAsync(request.Id, cancellationToken);
        if (purchaseOrder is null)
            return Result<Unit>.Failure(404, "Purchase order not found");

        if (purchaseOrder.Status is not (PurchaseOrderStatus.Issued or PurchaseOrderStatus.PartiallyReceived))
            return Result<Unit>.Failure(400, $"Cannot receive a PO with status {purchaseOrder.Status}");

        if (request.Lines.Count == 0)
            return Result<Unit>.Failure(400, "At least one line must be received");

        if (request.Lines.GroupBy(x => x.Id).Any(g => g.Count() > 1))
            return Result<Unit>.Failure(400, "Duplicate lines in request");

        var poLinesIds = purchaseOrder.Lines.Select(l => l.Id).ToHashSet();
        var invalidLine = request.Lines.FirstOrDefault(l => !poLinesIds.Contains(l.Id));
        if (invalidLine is not null)
            return Result<Unit>.Failure(400, $"Line {invalidLine.Id} does not belong to this PO");

        foreach(var received in request.Lines)
        {
            var poLine = purchaseOrder.Lines.First(l => l.Id == received.Id);

            if (received.Quantity <= 0)
                return Result<Unit>.Failure(400, $"Quantity for line {received.Id} must be greater than zero");
            if (received.Quantity > poLine.QuantityOrdered)
                return Result<Unit>.Failure(400, $"Cannot receive more than ordered for line {received.Id}");
        }

        foreach(var file in request.Files)
        {
            var blobUrl = await fileStorage.UploadAsync(
                file.Stream,
                file.FileName,
                file.ContentType,
                cancellationToken
            );

            purchaseOrder.AddDocument(
                file.FileName,
                file.ContentType,
                blobUrl,
                file.FileSizeBytes,
                currentUser.Id
            );
        }
        
        purchaseOrder.RecordReceipt([.. request.Lines.Select(l => (l.Id, l.Quantity))]);
        
        var saved = await purchaseOrderRepo.SaveChangesAsync(cancellationToken);

        if (!saved)
            return Result<Unit>.Failure(400, "Failed saving in database");

        await mediator.Publish(new PurchaseOrderReceivedEvent(
            request.Id,
            purchaseOrder.CreatedBy,
            currentUser.Id,
            DateTime.UtcNow,
            purchaseOrder.Number
        ), cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
};