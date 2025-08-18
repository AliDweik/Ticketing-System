using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data.Data;
using TicketingSystem.Data.Exceptions;
using TicketingSystem.Data.Models.Dashboard;
using TicketingSystem.Data.Repositories.Interfaces;

namespace TicketingSystem.Data.Repositories.Implements
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;
        public DashboardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<RecentComment>> GetRecentCommentsAsync(int count)
        {
            try
            {
                return await _context.TicketComments
                .OrderByDescending(c => c.CreatedAt)
                .Take(count)
                .Select(c => new RecentComment
                {
                    CommentId = c.Id,
                    TicketId = c.TicketId,
                    TicketTitle = c.Ticket.Title,
                    Content = c.Comment.Length > 50
                        ? c.Comment.Substring(0, 50) + "..."
                        : c.Comment,
                    CommentedAt = c.CreatedAt,
                    CommentedBy = c.CommentedBy.FullName
                })
                .ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<RecentTicket>> GetRecentTicketsAsync(int count)
        {
            try
            {
                return await _context.Tickets
                .OrderByDescending(t => t.CreatedAt)
                .Take(count)
                .Select(t => new RecentTicket
                {
                    TicketId = t.Id,
                    Title = t.Title,
                    Status = t.Status,
                    CreatedAt = t.CreatedAt,
                    CreatedBy = t.CreatedBy.FullName,
                    ProductName = t.Product.Name
                })
                .ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<TicketStatus>> GetStatusDistributionAsync()
        {
            try
            {
                var total = await _context.Tickets.CountAsync();

                return await _context.Tickets
                    .GroupBy(t => t.Status)
                    .Select(g => new TicketStatus
                    {
                        Status = g.Key,
                        Count = g.Count(),
                    })
                    .ToListAsync();
            }
            catch
            {
                throw;
            }

        }

        public async Task<DashboardSummary> GetSummaryAsync()
        {
            try
            {
                return new DashboardSummary
                {
                    TotalTickets = await _context.Tickets.CountAsync(),
                    InProgressTickets = await _context.Tickets.CountAsync(t => t.Status == Enums.TicketStatusEnum.InProgress),
                    ResolvedTickets = await _context.Tickets.CountAsync(t => t.Status == Enums.TicketStatusEnum.Closed),
                };
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<UserTicketCount>> GetTopSolversAsync(int count)
        {
            try
            {
                var total = await _context.Tickets.CountAsync(t => t.Status == Enums.TicketStatusEnum.Closed);

                return await _context.Tickets
                    .Where(t => t.Status == Enums.TicketStatusEnum.Closed)
                    .GroupBy(t => t.AssignedTo)
                    .Select(g => new UserTicketCount
                    {
                        UserId = g.Key.Id.ToString(),
                        UserName = g.Key.FullName,
                        Email = g.Key.Email,
                        TicketCount = g.Count(),
                        Percentage = total > 0 ? Math.Round(g.Count() * 100.0 / total, 1) : 0
                    })
                    .OrderByDescending(u => u.TicketCount)
                    .Take(count)
                    .ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<UserTicketCount>> GetTopSubmittersAsync(int count)
        {
            try
            {
                var total = await _context.Tickets.CountAsync();

                return await _context.Tickets
                    .GroupBy(t => t.CreatedBy)
                    .Select(g => new UserTicketCount
                    {
                        UserId = g.Key.Id.ToString(),
                        UserName = g.Key.FullName,
                        Email = g.Key.Email,
                        TicketCount = g.Count(),
                        Percentage = total > 0 ? Math.Round(g.Count() * 100.0 / total, 1) : 0
                    })
                    .OrderByDescending(u => u.TicketCount)
                    .Take(count)
                    .ToListAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
