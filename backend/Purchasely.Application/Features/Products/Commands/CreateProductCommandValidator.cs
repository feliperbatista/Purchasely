using FluentValidation;

namespace Purchasely.Application.Features.Products.Commands;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
         RuleFor(x => x.SKU)
            .MaximumLength(20).WithMessage("The maximum length for this field is 20 characters");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required")
            .MaximumLength(200).WithMessage("The maximum length for this field is 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("The maximum length for this field is 500 characters");

        RuleFor(x => x.Description)
            .MaximumLength(100).WithMessage("The maximum length for this field is 100 characters");
    }
}