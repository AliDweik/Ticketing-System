using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;
using TicketingSystem.API.Dtos;
using TicketingSystem.Data.Helpers;
using TicketingSystem.Data.Models.Auth;
using TicketingSystem.Data.Models.Ticketing;
using TicketingSystem.Data.Repositories.Interfaces;

namespace TicketingSystem.API.Controllers
{
    [Authorize(Policy = "UserActive")]
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketRepo _repo;
        public TicketController(ITicketRepo repo)
        {
            _repo = repo;
        }

        [HttpPost]
        [Authorize(Roles = "Client")]
        public async Task<ActionResult<TicketRequest>> AddTicket(TicketRequest ticket)
        {
            var ticketToCreate = new Ticket
            {
                Title = ticket.Title,
                ProblemDescription = ticket.ProblemDescription,
                CreatedAt = DateTime.Now,
                CreatedById = ticket.CreatedById,
                ProductId = ticket.ProductId,
            };

            await _repo.CreateTicket(ticketToCreate);
            
            return ticket;
        }
        
        [PolicyOrRole("UserWithTicket", "Admin")]
        [HttpGet("{ticketId}")]
        public async Task<ActionResult<TicketResponse>> GetTicket(Guid ticketId)
        {

            var ticket = await _repo.GetTicket(ticketId);

            var ticketResponse = new TicketResponse
            {
                Id = ticket.Id,
                Title = ticket.Title,
                ProblemDescription = ticket.ProblemDescription,
                Status = ticket.Status,
                CreatedAt = DateTime.Now,
                LastUpdateAt = ticket.LastUpdateAt,
                ProductId = ticket.ProductId,
                CreatedById = ticket.CreatedById,
                AssignedToId = ticket.AssignedToId,
                ClosedAt = ticket.ClosedAt,
            };

            return Ok(ticketResponse);
        }

        [PolicyOrRole("UserWithoutTicket", "Admin")]
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<TicketResponse>>> GetTicketsForUser(Guid userId)
        {
            var tickets = await _repo.GetTicketsForUser(userId);

            var ticketsResponse = new List<TicketResponse>();

            foreach (var ticket in tickets)
            {
                ticketsResponse.Add(
                    new TicketResponse
                    {
                        Id = ticket.Id,
                        Title = ticket.Title,
                        ProblemDescription = ticket.ProblemDescription,
                        Status = ticket.Status,
                        CreatedAt = DateTime.Now,
                        LastUpdateAt = ticket.LastUpdateAt,
                        ProductId = ticket.ProductId,
                        CreatedById = ticket.CreatedById,
                        AssignedToId = ticket.AssignedToId,
                        ClosedAt = ticket.ClosedAt,
                    });
            }

            return Ok(ticketsResponse);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketResponse>>> GetFilterdTickets([FromQuery] TicketFilter filter)
        {
            var tickets = await _repo.GetFilterdTickets(filter);
            var ticketsResponse = new List<TicketResponse>();

            foreach (var ticket in tickets)
            {
                ticketsResponse.Add(new TicketResponse
                    {
                        Id = ticket.Id,
                        Title = ticket.Title,
                        ProblemDescription = ticket.ProblemDescription,
                        Status = ticket.Status,
                        CreatedAt = DateTime.Now,
                        LastUpdateAt = ticket.LastUpdateAt,
                        ProductId = ticket.ProductId,
                        CreatedById = ticket.CreatedById,
                        AssignedToId = ticket.AssignedToId,
                        ClosedAt = ticket.ClosedAt,
                    }
                );
            }

            return Ok(ticketsResponse);
        }

        [Authorize(Policy = "UserWithoutTicket")]
        [HttpGet("{userId}/filterd")]
        public async Task<ActionResult<IEnumerable<TicketResponse>>> GetFilterdTicketsForUser(Guid userId, [FromQuery] TicketFilter filter)
        {
            var tickets = await _repo.GetFilterdTickets(userId, filter);
            var ticketsResponse = new List<TicketResponse>();

            foreach (var ticket in tickets)
            {
                ticketsResponse.Add(new TicketResponse
                {
                    Id = ticket.Id,
                    Title = ticket.Title,
                    ProblemDescription = ticket.ProblemDescription,
                    Status = ticket.Status,
                    CreatedAt = DateTime.Now,
                    LastUpdateAt = ticket.LastUpdateAt,
                    ProductId = ticket.ProductId,
                    CreatedById = ticket.CreatedById,
                    AssignedToId = ticket.AssignedToId,
                    ClosedAt = ticket.ClosedAt,
                }
                );
            }

            return Ok(ticketsResponse);
        }

        [Authorize(Policy = "UserWithTicket")]
        [HttpPut("{ticketId}")]
        public async Task<ActionResult<TicketResponse>> UpdateTicketStatus(Guid ticketId, string status)
        {
            var updatedTicket = await _repo.UpdateTicketStatus(ticketId, status);

            var ticketResponse = new TicketResponse
            {
                Id = updatedTicket.Id,
                Title = updatedTicket.Title,
                ProblemDescription = updatedTicket.ProblemDescription,
                Status = updatedTicket.Status,
                CreatedAt = DateTime.Now,
                LastUpdateAt = updatedTicket.LastUpdateAt,
                ProductId = updatedTicket.ProductId,
                CreatedById = updatedTicket.CreatedById,
                AssignedToId = updatedTicket.AssignedToId,
                ClosedAt = updatedTicket.ClosedAt,
            };

            return Ok(ticketResponse);
        }
    }
}
