using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data.Models.Auth;

namespace TicketingSystem.Data.Models.Ticketing
{
    public class TicketComment
    {
        public Guid Id { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Guid TicketId { get; set; }
        public Ticket Ticket { get; set; }

        public Guid CommentedById { get; set; }
        public User CommentedBy { get; set; }
    }
}
