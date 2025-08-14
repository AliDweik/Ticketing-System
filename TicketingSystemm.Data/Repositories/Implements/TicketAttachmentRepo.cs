using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data.Data;
using TicketingSystem.Data.Models.Ticketing;
using TicketingSystem.Data.Repositories.Interfaces;

namespace TicketingSystem.Data.Repositories.Implements
{
    public class TicketAttachmentRepo : ITicketAttachmentRepo
    {
        private readonly ApplicationDbContext _context;

        public TicketAttachmentRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TicketAttachment> AddAttachment(TicketAttachment attachment)
        {
            var ticket = await _context.Tickets.FindAsync(attachment.TicketId);

            if (ticket == null)
                throw new KeyNotFoundException("Ticket not found");

            var user = await _context.Users.FirstOrDefaultAsync(u => (u.Id == attachment.UploadedById));

            if (user == null)
                throw new InvalidOperationException("Invalid user");

            ticket.Attachments.Add(attachment);

            await _context.SaveChangesAsync();

            return attachment;
        }

        public async Task<TicketAttachment> GetAttachment(Guid attachmentId)
        {
            var attachment = await _context.TicketAttachments.FirstOrDefaultAsync(t => t.Id == attachmentId);

            if (attachment == null)
                throw new KeyNotFoundException("Ticket not found");

            return attachment;
        }

        public async Task<List<TicketAttachment>> GetAttachmentsForTicket(Guid ticketId)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Attachments)
                .FirstOrDefaultAsync(t => t.Id == ticketId);

            if (ticket == null)
                throw new KeyNotFoundException("Ticket not found");


            var attachments = ticket.Attachments.ToList();

            return attachments;
        }
    }
}
