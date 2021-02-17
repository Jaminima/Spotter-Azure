using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
            return View("Index");
        }

        [HttpGet("Log")]
        public IActionResult Log()
        {
            return View("Log");
        }

        #endregion Methods
    }
}
