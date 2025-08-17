using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data.Data;
using TicketingSystem.Data.Exceptions;
using TicketingSystem.Data.Models.Ticketing;
using TicketingSystem.Data.Repositories.Interfaces;

namespace TicketingSystem.Data.Repositories.Implements
{
    public class TicketCommentRepo : ITicketCommnetRepo
    {
        private readonly ApplicationDbContext _context;

        public TicketCommentRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TicketComment> AddComment(Guid ticketId, Guid userId, string content)
        {
            try
            {
                var ticket = await _context.Tickets.FindAsync(ticketId);

                if (ticket == null)
                    throw new KeyNotFoundException("Ticket not found");

                var user = await _context.Users.FirstOrDefaultAsync(u => (u.Id == userId));

                if (user == null)
                    throw new KeyNotFoundException("User not found");

                var ticketComment = new TicketComment
                {
                    Comment = content,
                    CommentedBy = user,
                    CommentedById = userId,
                    CreatedAt = DateTime.Now,
                    Ticket = ticket,
                    TicketId = ticketId
                };

                ticket.Comments.Add(ticketComment);

                await _context.SaveChangesAsync();
                return ticketComment;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<TicketComment>> GetCommentsForTicket(Guid ticketId)
        {
            try
            {
                var ticket = await _context.Tickets
                .Include(t => t.Comments)
                .FirstOrDefaultAsync(t => t.Id == ticketId);

                if (ticket == null)
                    throw new KeyNotFoundException("Ticket not found");

                return ticket.Comments.ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
