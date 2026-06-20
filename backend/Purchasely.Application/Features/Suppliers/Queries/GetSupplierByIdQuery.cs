using AutoMapper;
using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;

namespace Purchasely.Application.Features.Suppliers.Queries;

public record GetSupplierByIdQuery(Guid Id) : IRequest<Result<SupplierDetailsResponse>>;

public class GetSupplierByIdQueryHandler(
    ISupplierRepository repository,
    IMapper mapper
) : IRequestHandler<GetSupplierByIdQuery, Result<SupplierDetailsResponse>>
{
    public async Task<Result<SupplierDetailsResponse>> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
    {
        var supplier = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (supplier is null)
            return Result<SupplierDetailsResponse>.Failure(404, "Supplier not found");

        return Result<SupplierDetailsResponse>.Success(mapper.Map<SupplierDetailsResponse>(supplier));
    }
}