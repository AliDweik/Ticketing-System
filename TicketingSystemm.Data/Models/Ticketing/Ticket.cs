using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data.Dtos.Dashboard;
using TicketingSystem.Data.Enums;
using TicketingSystem.Data.Models.Auth;

namespace TicketingSystem.Data.Models.Ticketing
{
    public class Ticket
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string ProblemDescription { get; set; }
        public TicketStatusEnum Status { get; set; } = TicketStatusEnum.New;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastUpdateAt { get; set; }
        public DateTime? ClosedAt { get; set; }

        [ForeignKey("AssignedTo")]
        public Guid? AssignedToId { get; set; }
        public User? AssignedTo { get; set; }

        [ForeignKey("CreatedBy")]
        public Guid CreatedById { get; set; }
        public User CreatedBy { get; set; }

        [ForeignKey("Product")]
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public bool IsFixed { get; set; } = false;

        public ICollection<TicketComment> Comments { get; set; } = [];
        public ICollection<TicketAttachment> Attachments { get; set; } = [];

    }
}
