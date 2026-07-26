using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.Interfaces;

namespace Purchasely.Application.Features.Suppliers.Commands;

public record DeleteSupplierCommand(
    Guid Id
) : IRequest<Result<Unit>>;

public class DeleteSupplierCommandHandler(
    ISupplierRepository repository
) : IRequestHandler<DeleteSupplierCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (supplier is null)
            return Result<Unit>.Failure(404, "Supplier not found");

        repository.Delete(supplier);
        await repository.SaveChangesAsync(cancellationToken);
        
        return Result<Unit>.Success(Unit.Value);
    }
}