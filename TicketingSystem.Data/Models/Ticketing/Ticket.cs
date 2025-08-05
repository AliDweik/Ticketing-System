using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data.Models.Auth;

namespace TicketingSystem.Data.Models.Ticketing
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ProblemDescription { get; set; }
        public string Status { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastUpdateAt { get; set; }
        public DateTime ClosedAt { get; set; }

        public Guid AssignedToId { get; set; }
        public User AssignedTo { get; set; }

        public Guid CreatedById { get; set; }
        public User CreatedBy { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public ICollection<TicketComment> Comments { get; set; }
        public ICollection<TicketAttachment> Attachments { get; set; }

    }
}
