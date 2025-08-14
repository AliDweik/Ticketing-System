using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data.Dtos.Dashboard;
using TicketingSystem.Data.Enums;

namespace TicketingSystem.Data.Models.Ticketing
{
    public class TicketFilter
    {
        public TicketStatusEnum? Status { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? CreatedById { get; set; }
        public Guid? AssignedToId { get; set; }
        public string SortBy { get; set; } = "CreatedAt";
        public bool SortDescending { get; set; } = true;
    }
}
