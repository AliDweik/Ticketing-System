using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data.Models.Ticketing;

namespace TicketingSystem.Data.Repositories.Interfaces
{
    public interface ITicketAttachmentRepo
    {
        Task<TicketAttachment> AddAttachment(TicketAttachment attachment);

        Task<List<TicketAttachment>> GetAttachmentsForTicket(Guid ticketId);
        Task <TicketAttachment?> GetAttachment(Guid attachmentId);
    }
}
