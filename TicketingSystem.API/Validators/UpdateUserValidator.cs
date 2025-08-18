using FluentValidation;
using TicketingSystem.Data.Models.Auth;

namespace TicketingSystem.API.Validators
{
    public class UpdateUserValidator : AbstractValidator<UserUpdate>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.FullName)
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters");

            RuleFor(x => x.Image)
                .Must(image => image == null || image.Length <= 10 * 1024 * 1024)
                .WithMessage("Image size must be less than 10MB")
                .Must(image => image == null ||
                    new[] { ".jpg", ".jpeg", ".png" }.Contains(Path.GetExtension(image.FileName).ToLower()))
                .WithMessage("Only JPG and PNG images are allowed");

            RuleFor(x => x.MobileNumber)
                .Matches(@"^\+?[0-9]{10,15}$").WithMessage("Invalid mobile number format");

            RuleFor(x => x.Password)
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain at least one number");

            RuleFor(x => x.Address)
                .MaximumLength(200).WithMessage("Address cannot exceed 200 characters");
            
            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTime.Now.AddYears(-13)).WithMessage("Must be at least 13 years old")
                .GreaterThan(DateTime.Now.AddYears(-120)).WithMessage("Invalid date of birth");
        }
    }
}
