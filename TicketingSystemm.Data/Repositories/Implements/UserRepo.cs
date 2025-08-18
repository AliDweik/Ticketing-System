using Azure.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data.Data;
using TicketingSystem.Data.Dtos.Auth;
using TicketingSystem.Data.Enums;
using TicketingSystem.Data.Exceptions;
using TicketingSystem.Data.Helpers;
using TicketingSystem.Data.Models.Auth;
using TicketingSystem.Data.Repositories.Interfaces;

namespace TicketingSystem.Data.Repositories.Implements
{
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public UserRepo(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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

        public async Task<User?> UpdateUser(Guid id, UserUpdate user)
        {
            try
            {
                var userToUpdate = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (userToUpdate == null)
                    throw new KeyNotFoundException("There is no users");
                string error = "";
                if(user.FullName != null)
                {
                    var fullNameExists = await _context.Users.AnyAsync(u => u.FullName == user.FullName);
                    if (fullNameExists)
                        error += "User Name Is Exsists";
                    else
                        userToUpdate.FullName = user.FullName;

                }
                if (user.Email != null)
                {
                    var emailExists = await _context.Users.AnyAsync(u => u.Email == user.Email);
                    if (emailExists)
                        error += (error == "" ? "Email Is Exsists": ", Email Is Exsists");
                    else
                        userToUpdate.Email = user.Email;
                }
                if (user.Password != null)
                {
                    var passwordValidator = PasswordHelper.ValidatePassowrd(user.Password);
                    if(passwordValidator != null)
                        error += (error == "" ? passwordValidator : (", "+passwordValidator));
                    else
                    {
                        PasswordHelper.CreatePasswordHash(user.Password, out var passwordHash, out var passwordSalt);
                        userToUpdate.PasswordHash = passwordHash;
                        userToUpdate.PasswordSalt = passwordSalt;
                    }
                }
                if (user.Image != null)
                {
                    var uploadRoot = Path.Combine(_env.ContentRootPath, "UserImagesUploads");
                    Directory.CreateDirectory(uploadRoot);


                    var uniqueName = $"{Path.GetFileNameWithoutExtension(user.Image.FileName)}_{Guid.NewGuid():N}{Path.GetExtension(user.Image.FileName)}";
                    var path = Path.Combine(uploadRoot, uniqueName);

                    await using var stream = System.IO.File.Create(path);
                    await user.Image.CopyToAsync(stream);

                    userToUpdate.ImagePath = path;
                }

                if (user.Address != null)
                    userToUpdate.Address = user.Address;
                if(user.MobileNumber != null)
                    userToUpdate.MobileNumber = user.MobileNumber;
                if(user.DateOfBirth != null)
                    userToUpdate.DateOfBirth = (DateTime) user.DateOfBirth;

                if (error == "")
                    return userToUpdate;
                else
                    throw new AppException(error);
            }
            catch
            {
                throw;
            }
        }
    }
}
