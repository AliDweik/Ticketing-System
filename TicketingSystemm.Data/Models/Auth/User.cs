using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data.Enums;
using TicketingSystem.Data.Models.Ticketing;

namespace TicketingSystem.Data.Models.Auth
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]

        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MaxLength(20)]
        public string MobileNumber { get; set; }
        public string Address  { get; set; }
        public string UserImagePath { get; set; }

        [Required]
        public UserType UserType { get; set; }
        public bool IsActive { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Ticket> CreatedTickets { get; set; }
        public ICollection<Ticket> AssignedTickets { get; set; }
        public ICollection<TicketAttachment> Attachments { get; set; }
        public ICollection<TicketComment> Comments { get; set; }
    }
}
