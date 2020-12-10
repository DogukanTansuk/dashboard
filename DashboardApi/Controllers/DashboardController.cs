using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DashboardApi.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class DashboardController : ControllerBase
    {
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(ILogger<DashboardController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("/")]
        public ActionResult<Welcome> GetWelcome()
        {
            return new Welcome { Message = "Welcome"};
        }
    }

    public class Welcome
    {
        public string Message { get; init; }
    }
}
