using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data.Dtos.Auth;
using TicketingSystem.Data.Enums;
using TicketingSystem.Data.Models.Auth;

namespace TicketingSystem.Data.Repositories.Interfaces
{
    public interface IUserRepo
    {
        Task <User?> GetUserByEmail(string email);
        Task <User?> GetUserById(Guid userId);
        Task <IEnumerable<User>> GetUsersByType(UserType userType);
        Task <User?> UpdateUser(Guid id, UserUpdate user);
        Task ActivateUser(Guid userId);
        Task DeactivateUser(Guid userId);

    }
}
