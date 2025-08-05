using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data.Enums;
using TicketingSystem.Data.Models.Ticketing;

namespace TicketingSystem.Data.Models.Auth
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Address  { get; set; }
        public string UserImagePath { get; set; }
            
        public UserType UserType { get; set; }
        public bool IsActive { get; set; }
        
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Ticket> CreatedTickets { get; set; }
    }
}
