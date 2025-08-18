using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data.Data;
using TicketingSystem.Data.Enums;
using TicketingSystem.Data.Exceptions;
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
            try
            {
                var ticket = await _context.Tickets.FindAsync(ticketId);

                if (ticket == null)
                    throw new KeyNotFoundException("Ticket not found");

                var support = await _context.Users.FirstOrDefaultAsync(u => (u.Id == supportId && u.UserType == UserType.Support));

                if (support == null)
                    throw new KeyNotFoundException("User not found");
                if (support.IsActive == false)
                    throw new AppException("User in not active");

                ticket.AssignedToId = supportId;
                ticket.Status = TicketStatusEnum.InProgress;
                ticket.LastUpdateAt = DateTime.Now;

                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Ticket>> GetAssignedTickets(Guid supportId)
        {
            try
            {
                var support = await _context.Users
                    .Include(u => u.AssignedTickets)
                    .FirstOrDefaultAsync(u => u.Id == supportId);

                if (support == null)
                    throw new KeyNotFoundException("User not found");

                return support.AssignedTickets.ToList();
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> IsTicketAssigned(Guid ticketId)
        {
            try
            {
                var ticket = await _context.Tickets.FindAsync(ticketId);
                if (ticket == null)
                    throw new KeyNotFoundException("Ticket not found");

                if(ticket.AssignedToId == null)
                    return false;

                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}
