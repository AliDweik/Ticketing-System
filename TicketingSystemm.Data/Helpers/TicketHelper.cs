using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data.Models.Ticketing;

namespace TicketingSystem.Data.Helpers
{
    public static class TicketHelper
    {
        public static bool CanAddAttachment(Ticket ticket)
        {
            if(ticket.Status == Enums.TicketStatusEnum.Closed)
            {
                return false;
            }
            return true;
        }
    }
}
