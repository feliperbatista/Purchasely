using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.Interfaces;

namespace Purchasely.Application.Features.Suppliers.Commands;

public record UpdateSupplierCommand(
    Guid Id,
    string Name,
    string Email,
    string Phone,
    string TaxNumber,
    string Address
) : IRequest<Result<Unit>>;

public class UpdateSupplierCommandHandler(
    ISupplierRepository repository,
    ICacheService cache
) : IRequestHandler<UpdateSupplierCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (supplier is null)
            return Result<Unit>.Failure(404, "Supplier not found");

        supplier.Update(request.Name, request.Email, request.Phone, request.Address, request.TaxNumber);
        
        await repository.SaveChangesAsync(cancellationToken);
        
        await cache.RemoveAsync("suppliers:all", cancellationToken);

        return  Result<Unit>.Success(Unit.Value);
    }
}