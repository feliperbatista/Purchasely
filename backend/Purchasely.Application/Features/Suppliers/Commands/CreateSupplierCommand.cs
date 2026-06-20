using AutoMapper;
using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;

namespace Purchasely.Application.Features.Suppliers.Commands;

public record CreateSupplierCommand(
    string Name,
    string Email,
    string Phone,
    string TaxNumber,
    string Address
) : IRequest<Result<SupplierResponse>>;

public class CreateSupplierCommandHandler(
    ISupplierRepository repository,
    IMapper mapper
) : IRequestHandler<CreateSupplierCommand, Result<SupplierResponse>>
{
    public async Task<Result<SupplierResponse>> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        var existingSupplier = await repository.GetByTaxNumberAsync(request.TaxNumber, cancellationToken);
        if (existingSupplier is not null)
            return Result<SupplierResponse>.Failure(409, "TaxNumber already exists");

        var supplier = Supplier.Create(request.Name, request.Email, request.Phone, request.Address, request.TaxNumber);
        
        await repository.AddAsync(supplier, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return Result<SupplierResponse>.Success(mapper.Map<SupplierResponse>(supplier));
    }
}