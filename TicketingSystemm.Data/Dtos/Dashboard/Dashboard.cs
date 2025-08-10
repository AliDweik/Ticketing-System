namespace TicketingSystem.Data.Dtos.Dashboard
{
    public class Dashboard
    {
        public DashboardSummary Summary { get; set; }

        public List<TicketStatus> StatusDistribution { get; set; } = new();

        public List<RecentTicket> RecentTickets { get; set; } = new();
        public List<RecentComment> RecentComments { get; set; } = new();

        public List<UserTicketCount> TopSubmitters { get; set; } = new();
        public List<UserTicketCount> TopSolvers { get; set; } = new();
    }
}
