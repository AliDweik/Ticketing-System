using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data.Data;
using TicketingSystem.Data.Enums;
using TicketingSystem.Data.Exceptions;
using TicketingSystem.Data.Helpers;
using TicketingSystem.Data.Models.Auth;
using TicketingSystem.Data.Repositories.Interfaces;

namespace TicketingSystem.Data.Repositories.Implements
{
    public class AuthRepo : IAuthRepo
    {
        private readonly ApplicationDbContext _context;

        public AuthRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<User?> Login(string userName, string password)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => (u.FullName == userName || u.Email == userName));
                if (user == null)
                    throw new KeyNotFoundException("User not found");

                if (!PasswordHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                    return null;

                return await _context.Users
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(u => u.Id == user.Id);
            }
            catch
            {
                throw;
            }
        }

        public async Task<User> Register(User user, string password)
        {
            try
            {
                PasswordHelper.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

                user.PasswordSalt = passwordSalt;
                user.PasswordHash = passwordHash;

                string userType;
                if (user.UserType == UserType.Support)
                    userType = "Support";
                else if (user.UserType == UserType.Client)
                    userType = "Client";
                else
                {
                    throw new AppException("You can't register with this usertype");
                }

                // ToCheck
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == userType);

                var roleId = role!.Id;



                var userRole = new UserRole
                {
                    UserId = user.Id,
                    RoleId = roleId
                };

                user.UserRoles.Add(userRole);

                await _context.Users.AddAsync(user);
                await _context.UserRoles.AddAsync(userRole);
                await _context.SaveChangesAsync();

                return user;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UserExists(string userName)
        {
            try
            {
                return await _context.Users.AnyAsync(u => (u.FullName == userName || u.Email == userName));
            }
            catch
            {
                throw;
            }
        }
    }
}
