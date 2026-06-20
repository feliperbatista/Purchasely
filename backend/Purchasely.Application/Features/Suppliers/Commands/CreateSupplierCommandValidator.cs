using FluentValidation;

namespace Purchasely.Application.Features.Suppliers.Commands;

public class CreateSupplierCommandValidator : AbstractValidator<CreateSupplierCommand>
{
    public CreateSupplierCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Supplier name is required")
            .MaximumLength(200).WithMessage("The maximum length for this field is 200 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Supplier email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Supplier phone number is required")
            .MaximumLength(30).WithMessage("The maximum length for this field is 30 characters");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Supplier address is required")
            .MaximumLength(500).WithMessage("The maximum length for this field is 500 characters");

        RuleFor(x => x.TaxNumber)
            .NotEmpty().WithMessage("Tax number is required")
            .Matches(@"^\d{14}$")
            .WithMessage("Tax number must contain exactly 14 digits");
    }
}