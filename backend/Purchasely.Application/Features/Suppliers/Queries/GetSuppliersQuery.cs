using AutoMapper;
using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;

namespace Purchasely.Application.Features.Suppliers.Queries;

public record GetSuppliersQuery : IRequest<Result<List<SupplierListResponse>>>;

public class GetSuppliersQueryHandler(
    ISupplierRepository repository,
    IMapper mapper
) : IRequestHandler<GetSuppliersQuery, Result<List<SupplierListResponse>>>
{
    public async Task<Result<List<SupplierListResponse>>> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
    {
        var suppliers = await repository.GetAllAsync(cancellationToken);
        return Result<List<SupplierListResponse>>.Success(mapper.Map<List<SupplierListResponse>>(suppliers));
    }
}