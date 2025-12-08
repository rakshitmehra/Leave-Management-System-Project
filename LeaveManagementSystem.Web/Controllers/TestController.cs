using LeaveManagementSystem.Web.Models;

namespace LeaveManagementSystem.Web.Controllers
{
    /// <summary>
    /// This controller creates some test data and sends it to the view to demonstrate how MVC passes a model from      controller to view.
    /// </summary>
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            var data = new TestViewModel
            {
                Name = "Student of MVC Mastery",
                DateOfBirth = new DateTime(1954,12,01)
            };
            return View(data);
        }
    }
}
