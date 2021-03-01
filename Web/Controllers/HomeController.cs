using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.Models;
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

        public IActionResult Index([FromServices] SpotterAzure_dbContext dbContext)
        {
            Spotify sp = authDetails.CheckAuth(Request, dbContext);
            return View("Index", sp);
        }

        [HttpGet("Insights")]
        public IActionResult Insights([FromServices] SpotterAzure_dbContext dbContext)
        {
            Spotify sp = authDetails.CheckAuth(Request, dbContext);

            if (sp != null) return View("Insights", sp);
            else return Redirect("LoginError");
        }

        [HttpGet("Log")]
        public IActionResult Log([FromServices] SpotterAzure_dbContext dbContext)
        {
            Spotify sp = authDetails.CheckAuth(Request, dbContext);

            return View("Log", sp);
        }

        [HttpGet("LoginError")]
        public IActionResult LoginError()
        {
            return View("LoginError");
        }

        [HttpGet("Settings")]
        public IActionResult Settings([FromServices] SpotterAzure_dbContext dbContext)
        {
            Spotify sp = authDetails.CheckAuth(Request, dbContext);

            if (sp != null) return View("Settings", sp);
            else return Redirect("LoginError");
        }

        #endregion Methods
    }
}
