namespace TicketingSystem.Data.Models.Dashboard
{
    public class UserTicketCount
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int TicketCount { get; set; }
        public double Percentage { get; set; }
    }
}
