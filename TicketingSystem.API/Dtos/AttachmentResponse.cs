using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketingSystem.Data.Models.Auth;
using TicketingSystem.Data.Models.Ticketing;

namespace TicketingSystem.API.Dtos
{
    public class AttachmentResponse
    {   
        public Guid Id { get; set; }
        public string StoredFileName { get; set; }
        public DateTime UploadedAt { get; set; }
        public Guid TicketId { get; set; }
        public Guid UploadedById { get; set; }
        public string FilePath { get; set; }
    }
}
