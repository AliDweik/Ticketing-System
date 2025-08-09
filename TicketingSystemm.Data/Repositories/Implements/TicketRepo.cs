using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data.Data;
using TicketingSystem.Data.Enums;
using TicketingSystem.Data.Models.Auth;
using TicketingSystem.Data.Models.Ticketing;
using TicketingSystem.Data.Repositories.Interfaces;

namespace TicketingSystem.Data.Repositories.Implements
{
    public class TicketRepo:ITicketRepo
    {
        private readonly ApplicationDbContext _context;

        public TicketRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ticket>> GetFilterdTickets(TicketFilter ticketFilter)
        {
            var query = _context.Tickets
               .Include(t => t.Product)
               .Include(t => t.CreatedBy)
               .Include(t => t.AssignedTo)
               .AsQueryable();

            if (ticketFilter.Status != "")
                query = query.Where(t => t.Status == ticketFilter.Status);

            if (ticketFilter.ProductId.HasValue)
                query = query.Where(t => t.ProductId == ticketFilter.ProductId);

            if (ticketFilter.CreatedById.HasValue)
                query = query.Where(t => t.CreatedById == ticketFilter.CreatedById);

            if (ticketFilter.AssignedToId.HasValue)
                query = query.Where(t => t.AssignedToId == ticketFilter.AssignedToId);

            /*if (ticketFilter.SortBy != "")
                query = query.OrderBy($"{ticketFilter.SortBy} {(ticketFilter.SortDescending ? "desc" : "asc")}");
            else*/
                query = query.OrderByDescending(t => t.CreatedAt);
            

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetFilterdTickets(Guid userId, TicketFilter ticketFilter)
        {
            var query = _context.Tickets
               .Include(t => t.Product)
               .Include(t => t.CreatedById == userId)
               .Include(t => t.AssignedToId == userId)
               .AsQueryable();

            if (ticketFilter.Status != "")
                query = query.Where(t => t.Status == ticketFilter.Status);

            if (ticketFilter.ProductId.HasValue)
                query = query.Where(t => t.ProductId == ticketFilter.ProductId);

            if (ticketFilter.CreatedById.HasValue)
                query = query.Where(t => t.CreatedById == ticketFilter.CreatedById);

            if (ticketFilter.AssignedToId.HasValue)
                query = query.Where(t => t.AssignedToId == ticketFilter.AssignedToId);

            /*if (ticketFilter.SortBy != "")
                query = query.OrderBy($"{ticketFilter.SortBy} {(ticketFilter.SortDescending ? "desc" : "asc")}");
            else*/
            query = query.OrderByDescending(t => t.CreatedAt);


            return await query.ToListAsync();
        }


        public async Task<Ticket> GetTicket(Guid ticketId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);

            if (ticket == null)
                throw new KeyNotFoundException("Ticket not found");
            
            return ticket;
        }

        public async Task<IEnumerable<Ticket>> GetTicketsForUser(Guid userId)
        {
            return await _context.Tickets.Where(t => (t.CreatedById == userId || t.AssignedToId == userId)).ToListAsync();
        }

        public async Task<bool> IsTicketAssigner(Guid ticketId, Guid userId)
        {
            return await _context.Tickets.AnyAsync(t => (t.Id == ticketId && t.AssignedToId == userId));
        }

        public async Task<bool> IsTicketOwner(Guid ticketId, Guid userId)
        {
            return await _context.Tickets.AnyAsync(t => (t.Id == ticketId && t.CreatedById == userId));

        }

        public async Task<Ticket> UpdateTicketStatus(Guid ticketId, string status)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);

            if (ticket == null)
                throw new KeyNotFoundException("Ticket not found");

            ticket.Status = status;
            ticket.LastUpdateAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return ticket;
        }

        public async Task<Ticket> CreateTicket(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();

            return ticket;
        }
    }
}
