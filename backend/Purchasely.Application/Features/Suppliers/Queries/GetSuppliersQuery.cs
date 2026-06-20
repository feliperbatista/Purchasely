using AutoMapper;
using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;

namespace Purchasely.Application.Features.Suppliers.Queries;

public record GetSuppliersQuery : IRequest<Result<List<SupplierResponse>>>;

public class GetSuppliersQueryHandler(
    ISupplierRepository repository,
    IMapper mapper
) : IRequestHandler<GetSuppliersQuery, Result<List<SupplierResponse>>>
{
    public async Task<Result<List<SupplierResponse>>> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
    {
        var suppliers = await repository.GetAllAsync(cancellationToken);
        return Result<List<SupplierResponse>>.Success(mapper.Map<List<SupplierResponse>>(suppliers));
    }
}