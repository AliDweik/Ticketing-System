using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketingSystem.API.Dtos;
using TicketingSystem.Data.Helpers;
using TicketingSystem.Data.Models.Ticketing;
using TicketingSystem.Data.Repositories.Implements;
using TicketingSystem.Data.Repositories.Interfaces;

namespace TicketingSystem.API.Controllers
{
    [Authorize(Policy = "UserActive")]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ITicketCommnetRepo _repo;
        private readonly ITicketRepo _ticketRepo;

        public CommentController(ITicketCommnetRepo repo, ITicketRepo ticketRepo)
        {
            _repo = repo;
            _ticketRepo = ticketRepo;
        }

        [Authorize(Policy = "UserWithTicket")]
        [HttpPost("{ticketId}")]
        public async Task<ActionResult<CommentResponse>> AddComment(CommentRequest request)
        {
            var ticket = await _ticketRepo.GetTicket(request.TicketId);
            if (!TicketHelper.CanStart(ticket))
            {
                return BadRequest();
            }
            var createdComment = await _repo.AddComment(request.TicketId, request.CommentedById, request.Comment);

            var commentResponse = new CommentResponse
            {
                Id = createdComment.Id,
                Comment = createdComment.Comment,
                CommentedById = createdComment.CommentedById,
                TicketId = createdComment.TicketId,
                CreatedAt = createdComment.CreatedAt
            };

            return Ok(commentResponse);
        }

        [Authorize(Policy = "UserWithTicket")]
        [HttpGet("{ticketId}")]
        public async Task<ActionResult<List<CommentResponse>>> GetComment(Guid ticketId)
        {
            var comments = await _repo.GetCommentsForTicket(ticketId);
            
            var commentsResponse = new List<CommentResponse>();
            foreach (var comment in comments)
            {
                commentsResponse.Add(new CommentResponse
                {
                    Id = comment.Id,
                    Comment = comment.Comment,
                    CommentedById = comment.CommentedById,
                    TicketId = comment.TicketId,
                    CreatedAt = comment.CreatedAt
                });   
            }

            return Ok(commentsResponse);
        }
    }
}
