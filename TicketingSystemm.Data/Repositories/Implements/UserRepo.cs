using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data.Data;
using TicketingSystem.Data.Enums;
using TicketingSystem.Data.Exceptions;
using TicketingSystem.Data.Models.Auth;
using TicketingSystem.Data.Repositories.Interfaces;

namespace TicketingSystem.Data.Repositories.Implements
{
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDbContext _context;

        public UserRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ActivateUser(Guid userId)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                    throw new KeyNotFoundException("User not found");

                user.IsActive = true;

                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task DeactivateUser(Guid userId)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);

                if (user == null)
                    throw new KeyNotFoundException("User not found");

                user.IsActive = false;

                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                    throw new KeyNotFoundException("User not found");

                return user;
            }
            catch
            {
                throw;
            }
        }

        public async Task<User?> GetUserById(Guid userId)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                    throw new KeyNotFoundException("User not found");

                return user;
            }
            catch
            {
                throw;
            }

        }

        public async Task<IEnumerable<User>> GetUsersByType(UserType userType)
        {
            try
            {
                var users = await _context.Users.Where(u => u.UserType == userType).ToListAsync();
                if (users == null)
                    throw new KeyNotFoundException("There is no users");
                return users;
            }
            catch
            {
                throw;
            }
        }
    }
}
