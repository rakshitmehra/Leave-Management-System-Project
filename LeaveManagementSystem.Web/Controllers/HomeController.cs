using LeaveManagementSystem.Web.Models;
using System.Diagnostics;

namespace LeaveManagementSystem.Web.Controllers
{
    /// <summary>
    /// This controller handles basic site pages such as the home, privacy, and about views, and provides an error page that displays diagnostic information. It uses dependency-injected logging and returns simple views for each action.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Displays the home page of the application. 
        /// Placeholder for any business logic that should run when the homepage loads.
        /// </summary>

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Displays the application's privacy policy view.
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        /// <summary>
        /// Generates and displays an error page containing diagnostic information.
        /// Creates an ErrorViewModel with the current request ID for debugging purposes.
        /// Response caching is disabled so the error page is never served from cache.
        /// </summary>

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // queries
            // calculations
            var model = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(model);
        }
    }
}
