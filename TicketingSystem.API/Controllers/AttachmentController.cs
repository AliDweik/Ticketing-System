using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
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
        public async Task<ActionResult<AttachmentResponse>> GetAttachmentForTicket(Guid ticketId)
        {
            var attachments = await _repo.GetAttachmentsForTicket(ticketId);

            var attachmentsResponse = new List<AttachmentResponse> ();

            foreach(var attachment in attachments)
            {
                attachmentsResponse.Add(new AttachmentResponse
                {
                    Id = attachment.Id,
                    FileName = attachment.FileName,
                    TicketId = attachment.TicketId,
                    UploadedById = attachment.UploadedById,
                    UploadedAt = attachment.UploadedAt,
                });
            }
            
            return Ok(attachmentsResponse);
        }

        [Authorize(Policy = "Attachment")]
        [HttpGet("{attachmentId}/attachment")]
        public async Task<ActionResult<AttachmentResponse>> GetAttachment(Guid attachmentId)
        {
            var attachment = await _repo.GetAttachment(attachmentId);

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
