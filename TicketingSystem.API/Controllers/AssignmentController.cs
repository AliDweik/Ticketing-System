using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Data.Repositories.Interfaces;

namespace TicketingSystem.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly ITicketAssignmentRepo _repo;
        public AssignmentController(ITicketAssignmentRepo repo)
        {
            _repo = repo;
        }

        [HttpPost("{ticketId}/assign/{supportId}")]
        public async Task<ActionResult> AssignTicket(Guid ticketId, Guid supportId)
        {
            if (await _repo.IsTicketAssigned(ticketId))
                throw new Exception("Ticket is assigned");

            await _repo.AssignTicket(ticketId, supportId);

            return Ok();
        }

        [HttpPost("{ticketId}/reassign/{supportId}")]
        public async Task<ActionResult> ReassignTicket(Guid ticketId, Guid supportId)
        {
            await _repo.AssignTicket(ticketId, supportId);
            return Ok();
        }
    }
}
