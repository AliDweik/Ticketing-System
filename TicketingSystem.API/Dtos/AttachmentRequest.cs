using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketingSystem.Data.Models.Auth;
using TicketingSystem.Data.Models.Ticketing;

namespace TicketingSystem.API.Dtos
{
    public class AttachmentRequest
    {
        public List<IFormFile> Files { get; set; }
        public Guid TicketId { get; set; }
        public Guid UploadedById { get; set; }
    }
}
