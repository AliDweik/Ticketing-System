using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data.Dtos.Dashboard;

namespace TicketingSystem.Data.Repositories.Interfaces
{
    public interface IDashboardRepository
    {
        Task<DashboardSummary> GetSummaryAsync();
        Task<List<TicketStatus>> GetStatusDistributionAsync();
        Task<List<RecentTicket>> GetRecentTicketsAsync(int count = 5);
        Task<List<RecentComment>> GetRecentCommentsAsync(int count = 5);
        Task<List<UserTicketCount>> GetTopSubmittersAsync(int count = 5);
        Task<List<UserTicketCount>> GetTopSolversAsync(int count = 5);
    }
}
