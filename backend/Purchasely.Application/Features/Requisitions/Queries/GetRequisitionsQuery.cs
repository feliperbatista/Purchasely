using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;

namespace Purchasely.Application.Features.Requisitions.Queries;

public record GetRequisitionsQuery : IRequest<Result<List<RequisitionResponse>>>;

public class GetRequisitionsQueryHandler(
    IRequisitionRepository repository
) : IRequestHandler<GetRequisitionsQuery, Result<List<RequisitionResponse>>>
{
    public async Task<Result<List<RequisitionResponse>>> Handle(GetRequisitionsQuery request, CancellationToken cancellationToken)
    {
        var requisitions = await repository.GetAllAsync(cancellationToken);
        return Result<List<RequisitionResponse>>.Success([.. requisitions.Select(r =>
            new RequisitionResponse(r.Id,
                r.Number,
                r.Status,
                r.Priority,
                r.Justification,
                r.SubmittedAt,
                r.CreatedAt,
                r.Requester.Name,
                r.Lines.Select(l => new RequisitionLines(
                    l.Id,
                    l.Product.Name,
                    l.QuantityRequested,
                    l.EstimatedUnitPrice
                ))))]);
    }
}