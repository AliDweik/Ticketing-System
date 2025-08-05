using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data.Models.Auth;

namespace TicketingSystem.Data.Models.Ticketing
{
    public class TicketComment
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey("Ticket")]
        public Guid TicketId { get; set; }
        public Ticket Ticket { get; set; }

        [ForeignKey("CommentedBy")]
        public Guid CommentedById { get; set; }
        public User CommentedBy { get; set; }
    }
}
