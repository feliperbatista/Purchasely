using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;

namespace Purchasely.Application.Features.Requisitions.Queries;

public record GetRequisitionByIdQuery(Guid Id) : IRequest<Result<RequisitionResponse>>;

public class GetRequisitionByIdQueryHandler(
    IRequisitionRepository repository
) : IRequestHandler<GetRequisitionByIdQuery, Result<RequisitionResponse>>
{
    public async Task<Result<RequisitionResponse>> Handle(GetRequisitionByIdQuery request, CancellationToken cancellationToken)
    {
        var requisition = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (requisition is null)
            return Result<RequisitionResponse>.Failure(404, "Requisition not found");

        return Result<RequisitionResponse>.Success(new RequisitionResponse(requisition.Id,
                requisition.Number,
                requisition.Status,
                requisition.Priority,
                requisition.Justification,
                requisition.SubmittedAt,
                requisition.CreatedAt,
                requisition.Requester.Name,
                requisition.Requester.Id,
                requisition.Lines.Select(l => new RequisitionLines(
                    l.Id,
                    l.Product.Id,
                    l.Product.Name,
                    l.QuantityRequested,
                    l.EstimatedUnitPrice
                )),
                requisition.Approvals.Select(a => new Approvals(
                    a.Id,
                    a.Approver.Name,
                    a.ActionedAt
                ))));
    }
}