using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DashboardApi.Controllers
{
    [ApiController]
    [Route("cases")]
    public class DashboardController : ControllerBase
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly DashboardDBContext _dbContext;

        public DashboardController(ILogger<DashboardController> logger, DashboardDBContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("/")]
        public IActionResult Welcome()
        {
            return Ok(new {message = "Welcome"});
        }
        

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Case>>> GetAllCases()
        {
            return await _dbContext.Cases.ToListAsync();
        }
    }
}
