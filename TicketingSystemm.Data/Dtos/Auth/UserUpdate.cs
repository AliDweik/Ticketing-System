using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data.Enums;

namespace TicketingSystem.Data.Dtos.Auth
{
    public class UserUpdate
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? MobileNumber { get; set; }
        public string? Address { get; set; }
        public IFormFile? Image { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Password { get; set; }
    }
}
