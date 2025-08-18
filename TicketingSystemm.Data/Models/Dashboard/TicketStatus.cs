using TicketingSystem.Data.Enums;

namespace TicketingSystem.Data.Models.Dashboard
{
    public class TicketStatus
    {
        public TicketStatusEnum Status { get; set; }
        public int Count { get; set; }
    }
}
