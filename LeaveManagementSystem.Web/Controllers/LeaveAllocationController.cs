using LeaveManagementSystem.Application.Models.LeaveAllocations;
using LeaveManagementSystem.Application.Services.LeaveAllocations;
using LeaveManagementSystem.Application.Services.LeaveTypes;

namespace LeaveManagementSystem.Web.Controllers
{
    /// <summary>
    /// Controller that manages employee leave allocations. 
    /// Administrators can view employees, assign leave, and edit existing allocations.
    /// All users can view their own allocation details.
    /// This controller relies on dedicated services for business logic, keeping the controller clean and maintaining separation of concerns.
    /// </summary>

    [Authorize]
    public class LeaveAllocationController(ILeaveAllocationsService _leaveAllocationsService, ILeaveTypesService _leaveTypesService, ILogger <LeaveAllocationController> _logger) : Controller
    {
        /// <summary>
        /// Displays a list of all employees so the administrator can view or manage their leave allocations.
        /// </summary>

        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> Index()
        {
            var employees = await _leaveAllocationsService.GetEmployees();
            return View(employees);
        }

        /// <summary>
        /// Allocates leave for the specified employee and then redirects to the employee's allocation details page.
        /// </summary>
        /// <param name="id"></param>
        [Authorize(Roles = Roles.Administrator)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AllocateLeave(string? id)
        {
            await _leaveAllocationsService.AllocateLeave(id);
            return RedirectToAction(nameof(Details), new { userId = id });
        }

        /// <summary>
        /// Retrieves an existing leave allocation by its ID and displays it for editing.
        /// Returns NotFound if the ID is missing or the allocation cannot be found.
        /// </summary>
        /// <param name="id"></param>
        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> EditAllocation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allocation = await _leaveAllocationsService.GetEmployeeAllocation(id.Value);
            if (allocation == null)
            {
                return NotFound();
            }
            return View(allocation);
        }

        /// <summary>
        /// Validates and updates an employee's leave allocation. 
        /// Ensures the number of days does not exceed the maximum allowed for the leave type.
        /// If validation fails, reloads the original allocation data and returns the view.
        /// </summary>
        /// <param name="allocation"></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAllocation(LeaveAllocationEditVM allocation)
        {
            if (await _leaveTypesService.DaysExceedMaximum(allocation.LeaveType.Id, allocation.Days))
            {
                ModelState.AddModelError("Days", "The allocation exceeds the maximum leave type value");
            }

            if (ModelState.IsValid)
            {
                await _leaveAllocationsService.EditAllocation(allocation);
                return RedirectToAction(nameof(Details), new { userId = allocation.Employee.Id });
            }
            _logger.LogWarning("Leave Type Attempt Failed Due To Invalidity");

            var days = allocation.Days;
            allocation = await _leaveAllocationsService.GetEmployeeAllocation(allocation.Id);
            allocation.Days = days;
            return View(allocation);
        }

        /// <summary>
        /// Displays all leave allocations for a specific employee so the administrator can review them.
        /// </summary>
        /// <param name="userId"></param>
        public async Task<IActionResult> Details(string? userId)
        {
            var employeeVm = await _leaveAllocationsService.GetEmployeeAllocations(userId);
            return View(employeeVm);
        }
    }
}
