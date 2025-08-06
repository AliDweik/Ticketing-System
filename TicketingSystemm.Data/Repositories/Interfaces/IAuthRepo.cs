using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data.Enums;
using TicketingSystem.Data.Models.Auth;

namespace TicketingSystem.Data.Repositories.Interfaces
{
    public interface IAuthRepo
    {
        Task<User> Register(User user, string password, UserType userType);
        Task<User?> Login(string userName, string password);
        Task<bool> UserExists(string userName);
    }
}
