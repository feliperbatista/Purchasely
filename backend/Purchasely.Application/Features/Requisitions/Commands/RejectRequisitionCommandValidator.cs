using FluentValidation;

namespace Purchasely.Application.Features.Requisitions.Commands;

public class RejectRequisitionCommandValidator : AbstractValidator<RejectRequisitionCommand>
{
    public RejectRequisitionCommandValidator()
    {
         RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Reason is required")
            .MaximumLength(500).WithMessage("The maximum length for this field is 500 characters");
    }
}