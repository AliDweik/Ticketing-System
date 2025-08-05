using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data.Models.Auth;

namespace TicketingSystem.Data.Models.Ticketing
{
    public class TicketAttachment
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.Now; 

        public Guid TicketId { get; set; }
        public Ticket Ticket { get; set; }

        public Guid UploadedById {  get; set; }
        public User UploadedBy { get; set; }

    }
}
