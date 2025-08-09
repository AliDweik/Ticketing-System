using System.ComponentModel.DataAnnotations;
using TicketingSystem.Data.Enums;

namespace TicketingSystem.API.Dtos
{
    public class LoginRequest
    {
        public string FullName { get; set; }
        public string Password { get; set; }
    }
}
