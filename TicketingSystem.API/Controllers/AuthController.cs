using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data.Enums;
using TicketingSystem.Data.Models.Auth;
using TicketingSystem.Data.Repositories.Interfaces;
using TicketingSystem.API.Dtos;

namespace TicketingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepo _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepo repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(Dtos.LoginRequest request)
        {
            var user = await _repo.Login(request.FullName,request.Password);

            if (user == null)
                return BadRequest();

            if(!user.IsActive)
                return Unauthorized("Wait for your account to be activated");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
            };

            foreach (var userRole in user.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole.Role.Name));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(60),
                SigningCredentials = cred,
                Issuer = _config["JwtSettings:Issuer"],
                Audience = _config["JwtSettings:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                user = new
                {
                    userName = user.FullName,
                    userId = user.Id,
                    roles = user.UserRoles.Select(ur => ur.Role.Name).ToList()
                }
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Dtos.RegisterRequest request)
        {
            if(await _repo.UserExists(request.FullName))
                return BadRequest("Username already exisits");
            if (await _repo.UserExists(request.Email))
                return BadRequest("Email already exisits");

            var userToCreate = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                CreatedAt = DateTime.Now,
                MobileNumber = request.MobileNumber,
                DateOfBirth = request.DateOfBirth,
                Address = request.Address,
                UserType = request.UserType,
                IsActive = false
            };

            if(request.UserType == UserType.Client)
                userToCreate.IsActive = true;

            var user = await _repo.Register(userToCreate, request.Password);

            return Ok(user);
        }
    }
}
