using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DashboardApi.Models;
using DashboardApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DashboardApi.Controllers
{
    [Authorize]
    [Route("auth")]
    [ApiController]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly DashboardDBContext _dbContext;
        private readonly IAuthService _authService;

        public AuthController(DashboardDBContext dbContext, IAuthService authService)
        {
            _dbContext = dbContext;
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserDTO userDto)
        {
            if (userDto == null) return BadRequest();
            var authResponse = await _authService.Authenticate(userDto);
            if (authResponse == null) return Unauthorized(new { message = "Authentication Failure"});

            return Ok(authResponse);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDTO refreshTokenDto)
        {
            if (refreshTokenDto == null) return BadRequest();
            var token = await _dbContext.RefreshTokens.Include(t => t.User).SingleOrDefaultAsync(t => t.Token == refreshTokenDto.refresh_token);
            if (token == null || token.ValidUntil < DateTime.UtcNow || token.Expired)
            {
                return Unauthorized("Invalid Refresh Token");
            }

            var accessToken = _authService.GenerateAccessToken(new []{new Claim(ClaimTypes.Name, token.User.Email)});

            return Ok(new RefreshResponse {Token = accessToken});
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDto)
        {
            if (_dbContext.Users.SingleOrDefault(u => u.Email == userDto.email) != null)
                return BadRequest(new { message = "User Already Exists" });

            var user = await _authService.Register(userDto);
            if (user == null) return StatusCode(500);

            return Ok("Successfully registered please login!");
        }
    }

    public class RefreshResponse
    {
        public string Token { get; set; }
    }
}