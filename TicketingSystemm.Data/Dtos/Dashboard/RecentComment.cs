namespace TicketingSystem.Data.Dtos.Dashboard
{
    public class RecentComment
    {
        public Guid CommentId { get; set; }
        public Guid TicketId { get; set; }
        public string TicketTitle { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CommentedAt { get; set; }
        public string CommentedBy { get; set; } = string.Empty;
    }
}
