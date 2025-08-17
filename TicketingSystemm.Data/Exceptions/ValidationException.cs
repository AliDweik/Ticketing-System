using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.Data.Exceptions
{
    public class ValidationException : Exception
    {
        public IEnumerable<ValidationFailure> Errors { get; }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : base("Validation errors occurred")
        {
            Errors = failures;
        }
    }
}
