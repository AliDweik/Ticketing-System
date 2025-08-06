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

        public async Task<TicketAttachment> AddAttachment(Guid ticketId, Guid userId, string fileName, string filePath)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);

            if (ticket == null)
                throw new KeyNotFoundException("Ticket not found");

            var user = await _context.Users.FirstOrDefaultAsync(u => (u.Id == userId));

            if (user == null)
                throw new InvalidOperationException("Invalid user");

            var ticketAttathment = new TicketAttachment
            {
                FileName = fileName,
                FilePath = filePath,
                Ticket = ticket,
                TicketId = ticketId,
                UploadedAt = DateTime.Now,
                UploadedBy = user,
                UploadedById = userId
            };

            ticket.Attachments.Add(ticketAttathment);

            await _context.SaveChangesAsync();

            return ticketAttathment;
        }

        public async Task<TicketAttachment> GetAttachment(Guid attachmentId)
        {
            var attachment = await _context.TicketAttachments.FindAsync(attachmentId);

            if(attachment == null)
                throw new KeyNotFoundException("Attachment not found");

            return attachment;
        }

        public async Task<List<TicketAttachment>> GetAttachmentsForTicket(Guid ticketId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);

            if (ticket == null)
                throw new KeyNotFoundException("Attachment not found");

            return ticket.Attachments.ToList();
        }
    }
}
