using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketingSystem.Data.Models.Auth;
using TicketingSystem.Data.Models.Ticketing;

namespace TicketingSystem.API.Dtos
{
    public class TicketResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ProblemDescription { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdateAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public Guid? AssignedToId { get; set; }
        public Guid CreatedById { get; set; }
        public Guid ProductId { get; set; }
    }
}
