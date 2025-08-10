namespace TicketingSystem.Data.Dtos.Dashboard
{
    public class RecentTicket
    {
        public Guid TicketId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
    }
}
