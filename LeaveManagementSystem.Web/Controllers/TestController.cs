using LeaveManagementSystem.Application.Models;

namespace LeaveManagementSystem.Web.Controllers
{
    /// <summary>
    /// This controller creates some test data and sends it to the view to demonstrate how MVC passes a model from controller to view.
    /// </summary>
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            var data = new TestViewModel
            {
                Name = "Rakshit Mehra",
                DateOfBirth = new DateTime(2003, 07, 24)
            };
            return View(data);
        }
    }
}
