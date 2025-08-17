using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data.Enums;
using TicketingSystem.Data.Models.Auth;
using TicketingSystem.Data.Models.Ticketing;

namespace TicketingSystem.Data.Repositories.Interfaces
{
    public interface ITicketRepo
    {
        Task <Ticket?> GetTicket(Guid ticketId);

        Task <Ticket> CreateTicket(Ticket ticket);
        Task <IEnumerable<Ticket>> GetTicketsForUser(Guid userId);
        Task <IEnumerable<Ticket>> GetFilterdTickets(TicketFilter ticketFilter);
        Task <IEnumerable<Ticket>> GetFilterdTickets(Guid userId,TicketFilter ticketFilter);

        Task <Ticket?> UpdateTicketStatus(Guid ticketId, TicketStatusEnum status);

        Task FixTicket(Guid ticketId);
        Task <bool> IsTicketOwner(Guid ticketId, Guid userId);
        Task <bool> IsTicketAssigner(Guid ticketId, Guid userId);

    }
}
