using System.Threading.Tasks;
using DashboardApi.Models;
using DashboardApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DashboardApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DashboardContext _context;
        private readonly IAuthService _authService;

        public AuthController(DashboardContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserDTO userDto)
        {
            var token = _authService.Authenticate(userDto);
            if (token == null) return Unauthorized();

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserDTO userDto)
        {
            
        }
    }
}