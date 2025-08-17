using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Data.Exceptions;
using TicketingSystem.Data.Repositories.Interfaces;

namespace TicketingSystem.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly ITicketAssignmentRepo _repo;
        private ILogger<AssignmentController> _logger;
        public AssignmentController(ITicketAssignmentRepo repo, ILogger<AssignmentController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpPost("{ticketId}/assign/{supportId}")]
        public async Task<ActionResult> AssignTicket(Guid ticketId, Guid supportId)
        {
            if (await _repo.IsTicketAssigned(ticketId))
            {
                _logger.LogError("Ticket with ID " + ticketId + " is assigned");
                return StatusCode(400, new { Error = "Ticket is assigned" });
            }

            await _repo.AssignTicket(ticketId, supportId);
            _logger.LogInformation("Ticket with ID " + ticketId + " is assigned Successfully to " + supportId);
            return Ok("Ticket Assigned Successfully");
        }

        [HttpPost("{ticketId}/reassign/{supportId}")]
        public async Task<ActionResult> ReassignTicket(Guid ticketId, Guid supportId)
        {
            await _repo.AssignTicket(ticketId, supportId);
            _logger.LogInformation("Ticket with ID " + ticketId + " is reassigned Successfully to " + supportId);
            return Ok("Ticket Reassigned Successfully");
        }
    }
}
