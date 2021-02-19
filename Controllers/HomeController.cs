using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Spotter_Azure.Models;

namespace Spotter_Azure.Controllers
{
    

    [Route("")]
    public class HomeController : Controller
    {
        #region Fields

        private readonly ILogger<HomeController> _logger;

        #endregion Fields

        #region Constructors

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        #endregion Constructors

        #region Methods

        public IActionResult Index()
        {
            return View("Index", new authDetails(Request));
        }

        [HttpGet("Log")]
        public IActionResult Log()
        {
            return View("Log", new authDetails(Request));
        }

        [HttpGet("Insights")]
        public IActionResult Insights()
        {
            return View("Insights", new authDetails(Request));
        }

        #endregion Methods
    }
}
