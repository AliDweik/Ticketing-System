using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using TicketingSystem.API.Dtos;
using TicketingSystem.API.Validators;
using TicketingSystem.Data.Helpers;
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
        private readonly ITicketRepo _ticketRepo;
        private readonly IWebHostEnvironment _env;
        private readonly IValidator<AttachmentRequest> _validator;

        public AttachmentController(ITicketAttachmentRepo repo, ITicketRepo ticketRepo, IWebHostEnvironment env, IValidator <AttachmentRequest> validator)
        {
            _repo = repo;
            _ticketRepo = ticketRepo;
            _env = env;
            _validator = validator;
        }

        [Authorize(Policy = "UserWithTicket")]
        [HttpPost("{ticketId}")]
        public async Task<ActionResult> AddAttachment([FromForm] AttachmentRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.ToProblemDetails());

            var ticket = await _ticketRepo.GetTicket(request.TicketId);
            if (!TicketHelper.CanAddAttachment(ticket))
            {
                return BadRequest();
            }

            var uploadRoot = Path.Combine(_env.ContentRootPath, "Uploads");
            Directory.CreateDirectory(uploadRoot);

            var savedAttachments = new List<AttachmentResponse>();

            foreach (var file in request.Files)
            {
                var uniqueName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid():N}{Path.GetExtension(file.FileName)}";
                var path = Path.Combine(uploadRoot, uniqueName);

                await using var stream = System.IO.File.Create(path);
                await file.CopyToAsync(stream);

                var attach = new TicketAttachment
                {
                    OriginalFileName = file.FileName,
                    StoredFileName = uniqueName,
                    Size = file.Length,
                    FilePath = path,
                    UploadedAt = DateTime.Now,
                    UploadedById = request.UploadedById,
                    TicketId = request.TicketId,
                };
                var attachmentWithId = await _repo.AddAttachment(attach);
                var attachmentResponse = new AttachmentResponse
                {
                    StoredFileName = attachmentWithId.StoredFileName,
                    UploadedAt = attachmentWithId.UploadedAt,
                    TicketId = attachmentWithId.TicketId,
                    UploadedById = attachmentWithId.UploadedById,
                    FilePath = attachmentWithId.FilePath,
                    Id = attachmentWithId.Id
                };

                savedAttachments.Add(attachmentResponse);
            }

            return Ok(savedAttachments);
        }

        [PolicyOrRole("UserWithTicket","Admin")]
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
                    StoredFileName = attachment.StoredFileName,
                    TicketId = attachment.TicketId,
                    UploadedById = attachment.UploadedById,
                    UploadedAt = attachment.UploadedAt,
                    FilePath = attachment.FilePath
                });
            }
            
            return Ok(attachmentsResponse);
        }

        [PolicyOrRole("Attachment", "Admin")]
        [HttpGet("{attachmentId}/attachment")]
        public async Task<ActionResult<AttachmentResponse>> GetAttachment(Guid attachmentId)
        {
            var attachment = await _repo.GetAttachment(attachmentId);

            var attachmentResponse = new AttachmentResponse
                {
                    Id = attachment.Id,
                    StoredFileName = attachment.StoredFileName,
                    TicketId = attachment.TicketId,
                    UploadedById = attachment.UploadedById,
                    UploadedAt = attachment.UploadedAt,
                    FilePath = attachment.FilePath
            };

            return Ok(attachmentResponse);
        }
    }
}
