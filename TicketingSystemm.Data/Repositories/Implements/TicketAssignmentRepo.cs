using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data.Data;
using TicketingSystem.Data.Enums;
using TicketingSystem.Data.Models.Ticketing;
using TicketingSystem.Data.Repositories.Interfaces;

namespace TicketingSystem.Data.Repositories.Implements
{
    public class TicketAssignmentRepo : ITicketAssignmentRepo
    {
        private readonly ApplicationDbContext _context;

        public TicketAssignmentRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AssignTicket(Guid ticketId, Guid supportId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);

            if (ticket == null)
                throw new KeyNotFoundException("Ticket not found");
        
            var support = await _context.Users.FirstOrDefaultAsync(u => (u.Id == supportId && u.UserType == UserType.Support));

            if (support == null)
                throw new InvalidOperationException("Invalid support user");

            ticket.AssignedToId = supportId;
            ticket.Status = "Assigned";
            ticket.LastUpdateAt = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task<List<Ticket>> GetAssignedTickets(Guid supportId)
        {
            var support = await _context.Users.FirstOrDefaultAsync(u => u.Id == supportId);

            if (support == null)
                throw new InvalidOperationException("Invalid support user");

            return support.AssignedTickets.ToList();
        }
    }
}
