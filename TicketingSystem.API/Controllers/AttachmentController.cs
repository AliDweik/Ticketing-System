using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketingSystem.API.Dtos;
using TicketingSystem.Data.Models.Ticketing;
using TicketingSystem.Data.Repositories.Interfaces;

namespace TicketingSystem.API.Controllers
{
    [Authorize(Policy = "UserActive")]
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentController : ControllerBase
    {
        private readonly ITicketAttachmentRepo _repo;
        public AttachmentController(ITicketAttachmentRepo repo)
        {
            _repo = repo;
        }

        [Authorize(Policy = "UserWithTicket")]
        [HttpPost("{ticketId}")]
        public async Task<ActionResult<AttachmentResponse>> AddAttachment(AttachmentRequest request)
        {
            var createdAttachment = await _repo.AddAttachment(request.TicketId,request.UploadedById,request.FileName,request.FilePath);

            var attachment = new AttachmentResponse
            {
                Id = createdAttachment.Id,
                FileName = createdAttachment.FileName,
                TicketId = createdAttachment.TicketId,
                UploadedById = createdAttachment.UploadedById,
                UploadedAt = createdAttachment.UploadedAt,
            };

            return Ok(attachment);
        }

        [Authorize(Policy = "UserWithTicket")]
        [HttpGet("{ticketId}")]
        public async Task<ActionResult<AttachmentResponse>> GetAttachment(Guid ticketId)
        {
            var attachment = await _repo.GetAttachment(ticketId);

            var attachmentResponse = new AttachmentResponse
            {
                Id = attachment.Id,
                FileName = attachment.FileName,
                TicketId = attachment.TicketId,
                UploadedById = attachment.UploadedById,
                UploadedAt = attachment.UploadedAt,
            };

            return Ok(attachmentResponse);
        }
    }
}
