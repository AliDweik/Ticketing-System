using Azure.Core;
using Microsoft.AspNetCore.Hosting;
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
    public class TicketAttachmentRepo : ITicketAttachmentRepo
    {
        private readonly ApplicationDbContext _context;

        public TicketAttachmentRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TicketAttachment> AddAttachment(TicketAttachment attachment)
        {
            try
            {
                var ticket = await _context.Tickets.FindAsync(attachment.TicketId);

                if (ticket == null)
                    throw new KeyNotFoundException("Ticket not found");

                var user = await _context.Users.FirstOrDefaultAsync(u => (u.Id == attachment.UploadedById));

                if (user == null)
                    throw new KeyNotFoundException("User not found");

                ticket.Attachments.Add(attachment);

                await _context.SaveChangesAsync();

                return attachment;
            }
            catch
            {
                throw;
            }
        }

        public async Task<TicketAttachment?> GetAttachment(Guid attachmentId)
        {
            try
            {
                var attachment = await _context.TicketAttachments.FirstOrDefaultAsync(t => t.Id == attachmentId);

                if (attachment == null)
                    throw new KeyNotFoundException("Attachment not found");

                return attachment;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<TicketAttachment>> GetAttachmentsForTicket(Guid ticketId)
        {
            try
            {
                var ticket = await _context.Tickets
                .Include(t => t.Attachments)
                .FirstOrDefaultAsync(t => t.Id == ticketId);

                if (ticket == null)
                    throw new KeyNotFoundException("Ticket not found");

                var attachments = ticket.Attachments.ToList();

                return attachments;
            }
            catch
            {
                throw;
            }
        }
    }
}
