using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.Interfaces;

namespace Purchasely.Application.Features.Products.Commands;

public record UpdateProductCommand(
    Guid Id,
    string SKU,
    string Name,
    string? Description,
    string? Category
) : IRequest<Result<Unit>>;

public class UpdateProductCommandHandler(
    IProductRepository repository
) : IRequestHandler<UpdateProductCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (product is null)
            return Result<Unit>.Failure(404, "Product not found");

        product.Update(request.SKU, request.Name, request.Description, request.Category);
        
        bool saved = await repository.SaveChangesAsync(cancellationToken);
        
        return saved
            ? Result<Unit>.Success(Unit.Value)
            : Result<Unit>.Failure(400, "Failed saving in database");
    }
}