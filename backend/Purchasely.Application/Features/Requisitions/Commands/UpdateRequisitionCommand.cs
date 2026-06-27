using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;
using Purchasely.Domain.Enums;

namespace Purchasely.Application.Features.Requisitions.Commands;

public record UpdateRequisitionCommand(
    Guid Id,
    Priority Priority,
    string? Justification,
    List<CreateRequisitionLinesCommand> Lines
) : IRequest<Result<RequisitionResponse>>;

public class UpdateRequisitionCommandHandler(
    IRequisitionRepository requisitionRepo,
    IProductRepository productRepo,
    ICurrentUserService currentUserService
) : IRequestHandler<UpdateRequisitionCommand, Result<RequisitionResponse>>
{
    public async Task<Result<RequisitionResponse>> Handle(UpdateRequisitionCommand request, CancellationToken cancellationToken)
    {
        var requisition = await requisitionRepo.GetByIdAsync(request.Id, cancellationToken);
        if (requisition is null)
            return Result<RequisitionResponse>.Failure(404, "Requisition not found.");

        if (request.Lines is null || request.Lines.Count == 0)
            return Result<RequisitionResponse>.Failure(400, "Requisition must have at least one line.");

        if (requisition.Status != RequisitionStatus.Draft)
            return Result<RequisitionResponse>.Failure(400, "Only draft requisition can be edited.");

        if (requisition.RequesterId != currentUserService.Id)
            return Result<RequisitionResponse>.Failure(403, "You cannot edit someone else's requisition.");

        var productsId = request.Lines.Select(l => l.ProductId).Distinct().ToList();
        var products = await productRepo.GetByIdsAsync(productsId, cancellationToken);
        var foundIds = products.Select(p => p.Id).ToHashSet();

        var missingId = productsId.FirstOrDefault(id => !foundIds.Contains(id));
        if (missingId != default)
            return Result<RequisitionResponse>.Failure(400, $"Product {missingId} does not exist.");

        var productMap = products.ToDictionary(p => p.Id);
        
        var lines = request.Lines.Select(l =>
            RequisitionLine.Create(
                l.ProductId,
                l.QuantityRequested,
                l.EstimatedUnitPrice
            ))
            .ToList();
        requisition.Update(request.Priority, request.Justification, lines);
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
                )),
                requisition.Approvals.Select(a => new Approvals(
                    a.Id,
                    a.Approver.Name,
                    a.ActionedAt
                ))))
            : Result<RequisitionResponse>.Failure(400, "Failed saving in database");
    }
}