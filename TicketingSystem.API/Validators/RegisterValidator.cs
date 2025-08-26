using FluentValidation;
using TicketingSystem.API.Dtos;

namespace TicketingSystem.API.Validators
{
    public class RegisterValidator : AbstractValidator <RegisterRequest>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters");

            When(x => x.Image != null, () =>
            {
                RuleFor(x => x.Image)
                .Must(image => image == null || image.Length <= 10 * 1024 * 1024)
                .WithMessage("Image size must be less than 10MB")
                .Must(image => image == null ||
                    new[] { ".jpg", ".jpeg", ".png" }.Contains(Path.GetExtension(image.FileName).ToLower()))
                .WithMessage("Only JPG and PNG images are allowed");
            });
            
            RuleFor(x => x.MobileNumber)
                .NotEmpty().WithMessage("Mobile number is required")
                .Matches(@"^\+?[0-9]{10,15}$").WithMessage("Invalid mobile number format");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain at least one number");

            RuleFor(x => x.Address)
                .MaximumLength(200).WithMessage("Address cannot exceed 200 characters");

            RuleFor(x => x.UserType)
                .NotEmpty().WithMessage("User type is required")
                .IsInEnum().WithMessage("Invalid user type");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required")
                .LessThan(DateTime.Now.AddYears(-13)).WithMessage("Must be at least 13 years old")
                .GreaterThan(DateTime.Now.AddYears(-120)).WithMessage("Invalid date of birth");
        }
    }
}
