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
) : IRequest<Result<SupplierDetailsResponse>>;

public class CreateSupplierCommandHandler(
    ISupplierRepository repository
) : IRequestHandler<CreateSupplierCommand, Result<SupplierDetailsResponse>>
{
    public async Task<Result<SupplierDetailsResponse>> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        var existingSupplier = await repository.GetByTaxNumberAsync(request.TaxNumber, cancellationToken);
        if (existingSupplier is not null)
            return Result<SupplierDetailsResponse>.Failure(409, "TaxNumber already exists");

        var supplier = Supplier.Create(request.Name, request.Email, request.Phone, request.Address, request.TaxNumber);
        
        await repository.AddAsync(supplier, cancellationToken);
        bool saved = await repository.SaveChangesAsync(cancellationToken);

        return saved
            ? Result<SupplierDetailsResponse>.Success(new SupplierDetailsResponse(
                supplier.Id,
                supplier.Name,
                supplier.Email,
                supplier.Phone,
                supplier.TaxNumber,
                supplier.Address,
                supplier.IsActive,
                supplier.CreatedAt,
                Products: []
              ))
            : Result<SupplierDetailsResponse>.Failure(400, "Failed saving in database");
    }
}