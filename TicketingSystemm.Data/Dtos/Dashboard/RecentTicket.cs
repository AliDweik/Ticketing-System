using TicketingSystem.Data.Enums;

namespace TicketingSystem.Data.Dtos.Dashboard
{
    public class RecentTicket
    {
        public Guid TicketId { get; set; }
        public string Title { get; set; } = string.Empty;
        public TicketStatusEnum Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
    }
}
