using System;
using System.Threading.Tasks;
using DashboardApi.Models;
using DashboardApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DashboardApi.Controllers
{
    [Authorize]
    [Route("auth")]
    [ApiController]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] UserDto userDto)
        {
            if (userDto == null) return BadRequest();
            try
            {
                var authResponse = await _authService.Authenticate(userDto);
                return authResponse;
            }
            catch (ServiceException e)
            {
                return StatusCode(e.StatusCode, new {message = e.ErrorMessage});
            }
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<ActionResult<RefreshResponse>> Refresh([FromBody] RefreshTokenDto refreshTokenDto)
        {
            if (refreshTokenDto == null) return BadRequest();
            try
            {
                var accessToken = await _authService.RefreshAccessToken(refreshTokenDto);
                return new RefreshResponse {Token = accessToken};

            }
            catch (ServiceException e)
            {
                return StatusCode(e.StatusCode, new {message = e.ErrorMessage});
            }
            
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            try
            {
                await _authService.Register(userDto);
                return Ok(new { message ="Successfully registered please login!" });
            }
            catch (ServiceException e)
            {
                return StatusCode(e.StatusCode, new {message = e.ErrorMessage});
            }
        }
    }

    public class RefreshResponse
    {
        public string Token { get; set; }
    }
}