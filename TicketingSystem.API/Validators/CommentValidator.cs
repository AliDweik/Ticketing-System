using FluentValidation;
using TicketingSystem.API.Dtos;

namespace TicketingSystem.API.Validators
{
    public class CommentValidator : AbstractValidator<CommentRequest>
    {
        public CommentValidator()
        {
            RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("Comment is required")
            .MaximumLength(1000).WithMessage("Comment cannot exceed 1000 characters in one comment");

            RuleFor(x => x.TicketId)
                .NotEmpty().WithMessage("Ticket ID is required");

            RuleFor(x => x.CommentedById)
                .NotEmpty().WithMessage("Commenter ID is required");
        }
    }
}
