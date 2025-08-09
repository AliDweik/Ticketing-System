using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data.Models.Ticketing;

namespace TicketingSystem.Data.Repositories.Interfaces
{
    public interface ITicketAssignmentRepo
    {
        Task AssignTicket(Guid ticketId, Guid supportId);
        Task<List<Ticket>> GetAssignedTickets(Guid supportId);
        Task<bool> IsTicketAssigned(Guid ticketId);
    }
}
