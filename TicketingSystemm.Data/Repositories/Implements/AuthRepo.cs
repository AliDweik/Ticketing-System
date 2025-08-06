using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data.Data;
using TicketingSystem.Data.Enums;
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
            var user = await _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).FirstOrDefaultAsync(u => (u.FullName == userName || u.Email == userName));
            if (user == null)
                return null;

            if (!PasswordHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        public async Task<User> Register(User user, string password, UserType userType)
        {
            PasswordHelper.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
            
            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == userType.ToString());

            var roleId = role!.Id;

            
            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = roleId
            };

            await _context.UserRoles.AddAsync(userRole);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UserExists(string userName)
        {
            return await _context.Users.AnyAsync(u => (u.FullName == userName || u.Email == userName));
        }
    }
}
