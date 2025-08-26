using System.ComponentModel.DataAnnotations;
using TicketingSystem.Data.Enums;

namespace TicketingSystem.API.Dtos
{
    public class RegisterRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public IFormFile? Image { get; set; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public UserType UserType { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
