using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data.Models.Ticketing;

namespace TicketingSystem.Data.Repositories.Interfaces
{
    public interface ITicketCommnetRepo
    {
        Task<TicketComment> AddComment(Guid ticketId, Guid clientId, string content);

        Task<List<TicketComment>> GetCommentsForTicket(Guid ticketId);
    }
}
