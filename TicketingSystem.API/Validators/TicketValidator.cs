using FluentValidation;
using TicketingSystem.API.Dtos;

namespace TicketingSystem.API.Validators
{
    public class TicketValidator : AbstractValidator <TicketRequest>
    {
        public TicketValidator()
        {
            RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters");

            RuleFor(x => x.ProblemDescription)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters");

            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required");
        }
    }
}
