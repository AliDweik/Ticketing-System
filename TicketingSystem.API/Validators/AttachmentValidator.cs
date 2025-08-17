using FluentValidation;
using TicketingSystem.API.Dtos;

namespace TicketingSystem.API.Validators
{
    public class AttachmentValidator : AbstractValidator<AttachmentRequest>
    {
        public AttachmentValidator()
        {
            RuleFor(x => x.Files)
                .NotEmpty().WithMessage("At least one file is required")
                .Must(files => files.Count <= 5).WithMessage("Maximum 5 files allowed in one time")
                .ForEach(file =>
                {
                    file.Must(f => f.Length <= 10 * 1024 * 1024).WithMessage("File size must be less than 10MB");
                    file.Must(f =>
                        new[] { ".jpg", ".jpeg", ".png", ".pdf", ".docx" }
                        .Contains(Path.GetExtension(f.FileName).ToLower()))
                        .WithMessage("Only JPG, PNG, PDF, and DOCX files are allowed");
                });

            RuleFor(x => x.TicketId)
                .NotEmpty().WithMessage("Ticket ID is required");

            RuleFor(x => x.UploadedById)
                .NotEmpty().WithMessage("Uploader ID is required");
        }
    }
}
