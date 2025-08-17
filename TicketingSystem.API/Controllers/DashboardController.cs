using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Data.Dtos.Dashboard;
using TicketingSystem.Data.Exceptions;
using TicketingSystem.Data.Repositories.Implements;
using TicketingSystem.Data.Repositories.Interfaces;

namespace TicketingSystem.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/dash")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardRepository _repo;
        public DashboardController(IDashboardRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<Dashboard>> GetDashboardData()
        {
            try
            {
                var summaryTask = await _repo.GetSummaryAsync();
                var statusDistributionTask = await _repo.GetStatusDistributionAsync();
                var recentTicketsTask = await _repo.GetRecentTicketsAsync(5);
                var recentCommentsTask = await _repo.GetRecentCommentsAsync(5);
                var topSubmittersTask = await _repo.GetTopSubmittersAsync(5);
                var topSolversTask = await _repo.GetTopSolversAsync(5);
                
                return new Dashboard
                {
                    Summary = summaryTask,
                    StatusDistribution = statusDistributionTask,
                    RecentTickets = recentTicketsTask,
                    RecentComments = recentCommentsTask,
                    TopSubmitters = topSubmittersTask,
                    TopSolvers = topSolversTask,
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
