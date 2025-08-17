using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data.Data;
using TicketingSystem.Data.Enums;
using TicketingSystem.Data.Exceptions;
using TicketingSystem.Data.Models.Auth;
using TicketingSystem.Data.Models.Ticketing;
using TicketingSystem.Data.Repositories.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            try
            {
                var query = _context.Tickets
                    .Include(t => t.Product)
                    .Include(t => t.CreatedBy)
                    .Include(t => t.AssignedTo)
                    .AsQueryable();

                if (ticketFilter.Status != null)
                    query = query.Where(t => t.Status == ticketFilter.Status);

                if (ticketFilter.ProductId.HasValue)
                    query = query.Where(t => t.ProductId == ticketFilter.ProductId.Value);

                if (ticketFilter.CreatedById.HasValue)
                    query = query.Where(t => t.CreatedById == ticketFilter.CreatedById.Value);

                if (ticketFilter.AssignedToId.HasValue)
                    query = query.Where(t => t.AssignedToId == ticketFilter.AssignedToId.Value);

                query = ticketFilter.SortBy?.ToLower() switch
                {
                    "status" => ticketFilter.SortDescending ?
                        query.OrderByDescending(t => t.Status) :
                        query.OrderBy(t => t.Status),
                    "createdat" => ticketFilter.SortDescending ?
                        query.OrderByDescending(t => t.CreatedAt) :
                        query.OrderBy(t => t.CreatedAt),
                    _ => query.OrderByDescending(t => t.CreatedAt) // Default sort
                };

                return await query.ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<Ticket>> GetFilterdTickets(Guid userId, TicketFilter ticketFilter)
        {

            try
            {
                var query = _context.Tickets
               .Include(t => t.Product)
               .Include(t => t.CreatedById == userId)
               .Include(t => t.AssignedToId == userId)
               .AsQueryable();

                if (ticketFilter.Status != null)
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
            catch
            {
                throw;
            }
        }


        public async Task<Ticket?> GetTicket(Guid ticketId)
        {
            try
            {
                var ticket = await _context.Tickets.FindAsync(ticketId);

                if (ticket == null)
                    throw new KeyNotFoundException("Ticket not found");

                return ticket;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<Ticket>> GetTicketsForUser(Guid userId)
        {
            try
            {
                var tickets = await _context.Tickets.Where(t => (t.CreatedById == userId || t.AssignedToId == userId)).ToListAsync();
                if (tickets == null)
                    throw new KeyNotFoundException("There is no tickets");
                return tickets;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> IsTicketAssigner(Guid ticketId, Guid userId)
        {
            try
            {
                return await _context.Tickets.AnyAsync(t => (t.Id == ticketId && t.AssignedToId == userId));
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> IsTicketOwner(Guid ticketId, Guid userId)
        {
            try
            {
                return await _context.Tickets.AnyAsync(t => (t.Id == ticketId && t.CreatedById == userId));
            }
            catch
            {
                throw;
            }

        }

        public async Task<Ticket?> UpdateTicketStatus(Guid ticketId, TicketStatusEnum status)
        {
            try
            {
                var ticket = await _context.Tickets.FindAsync(ticketId);

                if (ticket == null)
                    throw new KeyNotFoundException("Ticket not found");

                if(ticket.Status == TicketStatusEnum.Closed && status != TicketStatusEnum.Closed)
                {
                    ticket.IsFixed = false;
                }

                ticket.Status = status;
                ticket.LastUpdateAt = DateTime.Now;
                ticket.ClosedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                return ticket;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Ticket> CreateTicket(Ticket ticket)
        {
            try
            {
                await _context.Tickets.AddAsync(ticket);
                await _context.SaveChangesAsync();

                return ticket;
            }
            catch
            {
                throw;
            }
        }

        public async Task FixTicket(Guid ticketId)
        {
            try
            {
                var ticket = await _context.Tickets.FindAsync(ticketId);
                if (ticket == null)
                    throw new KeyNotFoundException("Ticket not found");
                if (ticket.IsFixed == false)
                    ticket.IsFixed = true;
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
