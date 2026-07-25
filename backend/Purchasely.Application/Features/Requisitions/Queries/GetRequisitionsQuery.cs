using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Enums;

namespace Purchasely.Application.Features.Requisitions.Queries;

public record GetRequisitionsQuery(
    int Page = 1,
    int PageSize = 20,
    RequisitionStatus? Status = null,
    Priority? Priority = null,
    DateTime? From = null,
    DateTime? To = null,
    bool MyRequisitions = false
) : IRequest<Result<PaginatedResponse<RequisitionResponse>>>;

public class GetRequisitionsQueryHandler(
    IRequisitionRepository repository,
    ICurrentUserService currentUserService
) : IRequestHandler<GetRequisitionsQuery, Result<PaginatedResponse<RequisitionResponse>>>
{
    public async Task<Result<PaginatedResponse<RequisitionResponse>>> Handle(GetRequisitionsQuery request, CancellationToken cancellationToken)
    {
        var requisitions = await repository.GetAllAsync(
            request.Page,
            request.PageSize,
            request.Status,
            request.Priority,
            request.From,
            request.To,
            request.MyRequisitions ? currentUserService.Id : null,
            cancellationToken);

        var requisitionsCount = await repository.CountAsync(cancellationToken);

        return Result<PaginatedResponse<RequisitionResponse>>.Success(new PaginatedResponse<RequisitionResponse>(
            Items: [.. requisitions.Select(r =>
            new RequisitionResponse(r.Id,
                r.Number,
                r.Status,
                r.Priority,
                r.Justification,
                r.SubmittedAt,
                r.CreatedAt,
                r.Requester.Name,
                r.Requester.Id,
                r.Lines.Select(l => new RequisitionLines(
                    l.Id,
                    l.Product.Name,
                    l.QuantityRequested,
                    l.EstimatedUnitPrice
                )), 
                null))],
            request.Page,
            request.PageSize,
            requisitionsCount
        ));
    }
}