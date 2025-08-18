using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TicketingSystem.API.Dtos;
using TicketingSystem.API.Validators;
using TicketingSystem.Data.Helpers;
using TicketingSystem.Data.Models.Auth;
using TicketingSystem.Data.Repositories.Interfaces;

namespace TicketingSystem.API.Controllers
{
    [Authorize]
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _repo;
        private readonly IValidator <UserUpdate> _validator;
        public UserController(IUserRepo repo, IValidator <UserUpdate> validator)
        {
            _repo = repo;
            _validator = validator;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{userId}/activate")]
        public async Task<IActionResult> ActivateUser(Guid userId)
        {
            await _repo.ActivateUser(userId);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{userId}/deactivate")]
        public async Task<IActionResult> DeactivateUser(Guid userId)
        {
            await _repo.DeactivateUser(userId);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
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
                    ImagePath = client.ImagePath,
                    Address = client.Address,
                    UserType = client.UserType,
                    CreatedAt = client.CreatedAt,
                    DateOfBirth = client.DateOfBirth
                });
            }

            return Ok(clientsRepsonse);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("supports")]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetSupports()
        {
            var supports = await _repo.GetUsersByType(Data.Enums.UserType.Support);
            var supportsRepsonse = new List<UserResponse>();

            foreach (var support in supports)
            {
                supportsRepsonse.Add(new UserResponse
                {
                    Id = support.Id,
                    FullName = support.FullName,
                    Email = support.Email,
                    MobileNumber = support.MobileNumber,
                    ImagePath = support.ImagePath,
                    Address = support.Address,
                    UserType = support.UserType,
                    CreatedAt = support.CreatedAt,
                    DateOfBirth = support.DateOfBirth
                });
            }

            return Ok(supportsRepsonse);
        }

        [PolicyOrRole("UserWithoutTicket","Admin")]
        [HttpPut("{userId}/update")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromForm] UserUpdate user)
        {
            var validationResult = await _validator.ValidateAsync(user);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.ToProblemDetails());

            await _repo.UpdateUser(userId, user);
            return Ok("User Updated Successfully");
        }
    }
}
