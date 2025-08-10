using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.Data.Dtos.Dashboard
{
    public class DashboardSummary
    {
        public int TotalTickets { get; set; }
        public int OpenTickets { get; set; }
        public int InProgressTickets { get; set; }
        public int ResolvedTickets { get; set; }
    }
}
