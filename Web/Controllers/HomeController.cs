using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Spotter_Azure.Models;
using Model.Models;

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
            Spotify sp = authDetails.CheckAuth(Request, Model.Models.SpotterAzure_dbContext.dbContext);
            return View("Index", sp);
        }

        [HttpGet("LoginError")]
        public IActionResult LoginError()
        {
            return View("LoginError");
        }

        [HttpGet("Insights")]
        public IActionResult Insights()
        {
            Spotify sp = authDetails.CheckAuth(Request, Model.Models.SpotterAzure_dbContext.dbContext);

            if (sp != null) return View("Insights", sp);
            else return Redirect("LoginError");
        }

        [HttpGet("Log")]
        public IActionResult Log()
        {
            Spotify sp = authDetails.CheckAuth(Request, Model.Models.SpotterAzure_dbContext.dbContext);

            return View("Log", sp);
        }

        [HttpGet("Settings")]
        public IActionResult Settings()
        {
            Spotify sp = authDetails.CheckAuth(Request, Model.Models.SpotterAzure_dbContext.dbContext);

            if (sp != null) return View("Settings", sp);
            else return Redirect("LoginError");
        }

        #endregion Methods
    }
}
