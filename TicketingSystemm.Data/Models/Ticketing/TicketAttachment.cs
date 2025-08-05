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
    public class TicketAttachment
    {
        [Key]
        public Guid Id { get; set; }
        public string FileName { get; set; } = "New File";
        [Required]
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.Now;

        [ForeignKey("Ticket")]
        public Guid TicketId { get; set; }
        public Ticket Ticket { get; set; }

        [ForeignKey("UploadedBy")]
        public Guid UploadedById {  get; set; }
        public User UploadedBy { get; set; }

    }
}
