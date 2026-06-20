using AutoMapper;
using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;

namespace Purchasely.Application.Features.Suppliers.Queries;

public record GetSupplierByIdQuery(Guid Id) : IRequest<Result<SupplierResponse>>;

public class GetSupplierByIdQueryHandler(
    ISupplierRepository repository,
    IMapper mapper
) : IRequestHandler<GetSupplierByIdQuery, Result<SupplierResponse>>
{
    public async Task<Result<SupplierResponse>> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
    {
        var supplier = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (supplier is null)
            return Result<SupplierResponse>.Failure(404, "Supplier not found");

        return Result<SupplierResponse>.Success(mapper.Map<SupplierResponse>(supplier));
    }
}