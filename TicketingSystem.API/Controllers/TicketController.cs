using Azure.Core;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;
using TicketingSystem.API.Dtos;
using TicketingSystem.API.Validators;
using TicketingSystem.Data.Enums;
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
        private readonly IValidator<TicketRequest> _validator;
        public TicketController(ITicketRepo repo, IValidator <TicketRequest> validator)
        {
            _repo = repo;
            _validator = validator;
        }

        [HttpPut("{ticketId}/fix")]
        [Authorize(Roles = "Client", Policy = "UserWithTicket")]
        public async Task<ActionResult> FixTicket(Guid ticketId)
        {
            await _repo.FixTicket(ticketId);
            return Ok("Ticket Fixed Successfully");
        }

        [HttpPost]
        [Authorize(Roles = "Client")]
        public async Task<ActionResult<TicketRequest>> AddTicket(TicketRequest ticket)
        {
            var validationResult = await _validator.ValidateAsync(ticket);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.ToProblemDetails());

            var ticketToCreate = new Ticket
            {
                Title = ticket.Title,
                ProblemDescription = ticket.ProblemDescription,
                CreatedAt = DateTime.Now,
                CreatedById = ticket.CreatedById,
                ProductId = ticket.ProductId,
                Status = TicketStatusEnum.New
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
                IsFixed = ticket.IsFixed,
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
                        IsFixed = ticket.IsFixed,
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
                        IsFixed= ticket.IsFixed,
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
                    IsFixed = ticket.IsFixed,
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

        [Authorize(Roles = "Support", Policy = "UserWithTicket")]
        [HttpPut("{ticketId}")]
        public async Task<ActionResult<TicketResponse>> UpdateTicketStatus(Guid ticketId, TicketStatusEnum status)
        {
            if(status != TicketStatusEnum.InProgress || status != TicketStatusEnum.Closed)
            {
                return BadRequest("Status is not applicable");
            }

            var ticket = await _repo.GetTicket(ticketId);

            if(status == TicketStatusEnum.Closed && ticket.IsFixed == false)
            {
                return BadRequest("Ticket is not fixed");
            }

            await _repo.UpdateTicketStatus(ticketId, status);

            var ticketResponse = new TicketResponse
            {
                Id = ticket.Id,
                Title = ticket.Title,
                ProblemDescription = ticket.ProblemDescription,
                Status = ticket.Status,
                IsFixed = ticket.IsFixed,
                CreatedAt = DateTime.Now,
                LastUpdateAt = ticket.LastUpdateAt,
                ProductId = ticket.ProductId,
                CreatedById = ticket.CreatedById,
                AssignedToId = ticket.AssignedToId,
                ClosedAt = ticket.ClosedAt,
            };

            return Ok(ticketResponse);
        }
    }
}
