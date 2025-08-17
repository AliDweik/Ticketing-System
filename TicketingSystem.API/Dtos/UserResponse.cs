using System.ComponentModel.DataAnnotations;
using TicketingSystem.Data.Enums;

namespace TicketingSystem.API.Dtos
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string? ImagePath { get; set; }
        public string Address { get; set; }
        public UserType UserType { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
