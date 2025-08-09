using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TicketingSystem.API.Dtos;
using TicketingSystem.Data.Models.Auth;
using TicketingSystem.Data.Repositories.Interfaces;

namespace TicketingSystem.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _repo;
        
        public UserController(IUserRepo repo)
        {
            _repo = repo;
        }

        [HttpPut("{userId}/activate")]
        public async Task<IActionResult> ActivateUser(Guid userId)
        {
            await _repo.ActivateUser(userId);
            return NoContent();
        }

        [HttpPut("{userId}/deactivate")]
        public async Task<IActionResult> DeactivateUser(Guid userId)
        {
            await _repo.DeactivateUser(userId);
            return NoContent();
        }

        [HttpGet("clients")]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetClients()
        {
            var clients = await _repo.GetUsersByType(Data.Enums.UserType.Client);
            var clientsRepsonse = new List<UserResponse>();

            foreach (var client in clients)
            {
                clientsRepsonse.Add(new UserResponse
                {
                    Id = client.Id,
                    FullName = client.FullName,
                    Email = client.Email,
                    MobileNumber = client.MobileNumber,
                    Address = client.Address,
                    UserType = client.UserType,
                    CreatedAt = client.CreatedAt,
                    DateOfBirth = client.DateOfBirth
                });
            }

            return Ok(clientsRepsonse);
        }

        [HttpGet("supports")]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetSupports()
        {
            var supports = await _repo.GetUsersByType(Data.Enums.UserType.Support);
            var supportsRepsonse = new List<UserResponse>();

            foreach (var support in supportsRepsonse)
            {
                supportsRepsonse.Add(new UserResponse
                {
                    Id = support.Id,
                    FullName = support.FullName,
                    Email = support.Email,
                    MobileNumber = support.MobileNumber,
                    Address = support.Address,
                    UserType = support.UserType,
                    CreatedAt = support.CreatedAt,
                    DateOfBirth = support.DateOfBirth
                });
            }

            return Ok(supportsRepsonse);
        }
    }
}
