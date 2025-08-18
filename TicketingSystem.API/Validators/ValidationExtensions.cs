using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace TicketingSystem.API.Validators
{
    public static class ValidationExtensions
    {
        public static ProblemDetails ToProblemDetails(this ValidationResult result)
        {
            return new ValidationProblemDetails(
                result.Errors.GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    ))
            {
                Title = "Validation failed",
                Detail = "One or more validation errors occurred",
                Status = StatusCodes.Status400BadRequest
            };
        }
    }
}
