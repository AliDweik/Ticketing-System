using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketingSystem.Data.Models.Auth;
using TicketingSystem.Data.Models.Ticketing;

namespace TicketingSystem.API.Dtos
{
    public class TicketRequest
    {
        public string Title { get; set; }
        public string ProblemDescription { get; set; }
        public Guid CreatedById { get; set; }
        public Guid ProductId { get; set; }
    }
}
