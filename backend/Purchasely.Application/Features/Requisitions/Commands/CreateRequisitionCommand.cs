using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;
using Purchasely.Domain.Enums;

namespace Purchasely.Application.Features.Requisitions.Commands;

public record CreateRequisitionCommand(
    RequisitionStatus Status,
    Priority Priority,
    string? Justification,
    List<CreateRequisitionLinesCommand> Lines
) : IRequest<Result<RequisitionResponse>>;

public class CreateRequisitionCommandHandler(
    IRequisitionRepository requisitionRepo,
    IProductRepository productRepo,
    ICurrentUserService currentUserService
) : IRequestHandler<CreateRequisitionCommand, Result<RequisitionResponse>>
{
    public async Task<Result<RequisitionResponse>> Handle(CreateRequisitionCommand request, CancellationToken cancellationToken)
    {
        if (request.Lines is null || request.Lines.Count == 0)
            return Result<RequisitionResponse>.Failure(400, "Requisition must have at least one line.");

        var productsId = request.Lines.Select(l => l.ProductId).Distinct().ToList();
        var products = await productRepo.GetByIdsAsync(productsId, cancellationToken);
        var foundIds = products.Select(p => p.Id).ToHashSet();

        var missingId = productsId.FirstOrDefault(id => !foundIds.Contains(id));
        if (missingId != default)
            return Result<RequisitionResponse>.Failure(400, $"Product {missingId} does not exist.");

        var productMap = products.ToDictionary(p => p.Id);

        var requisition = Requisition.Create(
            request.Status,
            request.Priority,
            request.Justification,
            currentUserService.Id,
            [.. request.Lines.Select(l => 
                RequisitionLine.Create(
                    l.ProductId,
                    l.QuantityRequested,
                    l.EstimatedUnitPrice))]
        );

        await requisitionRepo.AddAsync(requisition, cancellationToken);
        var saved = await requisitionRepo.SaveChangesAsync(cancellationToken);

        return saved 
            ? Result<RequisitionResponse>.Success(new RequisitionResponse(
                requisition.Id,
                requisition.Number,
                requisition.Status,
                requisition.Priority,
                requisition.Justification,
                requisition.SubmittedAt,
                requisition.CreatedAt,
                currentUserService.Name,
                requisition.Lines.Select(l => new RequisitionLines(
                    l.Id,
                    productMap[l.ProductId].Name,
                    l.QuantityRequested,
                    l.EstimatedUnitPrice
                ))))
            : Result<RequisitionResponse>.Failure(400, "Failed saving in database");
    }
}