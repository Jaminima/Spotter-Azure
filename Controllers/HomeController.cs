using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace Spotter_Azure.Controllers
{
    public class authDetails
    {
        public string authToken, spotid;

        public authDetails(HttpRequest request)
        {
            authToken = request.Cookies["authToken"];
            spotid = request.Cookies["spotid"];
        }
    }

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

        [HttpGet("ClearCookie")]
        public IActionResult ClearCookie()
        {
            foreach (string k in HttpContext.Request.Cookies.Keys)
            {
                HttpContext.Response.Cookies.Delete(k);
            }
            return RedirectPermanent("/");
        }

        #endregion Methods
    }
}
