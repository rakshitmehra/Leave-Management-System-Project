using LeaveManagementSystem.Application.Models.LeaveRequests;
using LeaveManagementSystem.Application.Services.LeaveRequests;
using LeaveManagementSystem.Application.Services.LeaveTypes;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeaveManagementSystem.Web.Controllers
{
    /// <summary>
    /// Handles all employee and administrator interactions related to leave requests, including viewing personal leave requests, submitting new requests, canceling requests, listing all requests for administrators, and reviewing or approving/declining submitted requests.
    /// </summary>
    /// <param name="_leaveTypesService"></param>
    /// <param name="_leaveRequestsService"></param>
    [Authorize]
    public class LeaveRequestsController(ILeaveTypesService _leaveTypesService, ILeaveRequestsService _leaveRequestsService) : Controller
    {
        /// <summary>
        /// Retrieves and displays the logged-in employee’s list of leave requests, including their status and details.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var model = await _leaveRequestsService.GetEmployeeLeaveRequests();
            return View(model);
        }

        /// <summary>
        /// Displays the form used by employees to create a new leave request.
        /// Pre-fills the dates and optionally selects a leave type if one is provided.
        /// </summary>
        /// <param name="leaveTypeId"></param>
        public async Task<IActionResult> Create(int? leaveTypeId)
        {
            var leaveTypes = await _leaveTypesService.GetAll();
            var leaveTypesList = new SelectList(leaveTypes, "Id", "Name", leaveTypeId);
            var model = new LeaveRequestCreateVM
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                LeaveTypes = leaveTypesList
            };
            return View(model);
        }

        /// <summary>
        /// Processes the submission of a new leave request.
        /// Validates date ranges against available leave allocation and, if valid, creates the request; otherwise returns errors to the user.
        /// </summary>
        /// <param name="model"></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveRequestCreateVM model)
        {
            if (await _leaveRequestsService.RequestDatesExceedAllocation(model))
            {
                ModelState.AddModelError(string.Empty, "You have exceeded your allocation");
                ModelState.AddModelError(nameof(model.EndDate), "The number of days requested is invalid");
            }
            if (ModelState.IsValid)
            {
                await _leaveRequestsService.CreateLeaveRequest(model);
                return RedirectToAction(nameof(Index));
            }

            var leaveTypes = await _leaveTypesService.GetAll();
            model.LeaveTypes = new SelectList(leaveTypes, "Id", "Name");

            return View(model);
        }

        /// <summary>
        /// Cancels an existing leave request for the current employee and redirects back to the request list.
        /// </summary>
        /// <param name="id"></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            await _leaveRequestsService.CancelLeaveRequest(id);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Retrieves and displays all leave requests from all employees, summarizing counts by status (pending, approved, declined, etc.).
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "AdminSupervisorOnly")]
        public async Task<IActionResult> ListRequests()
        {
            var model = await _leaveRequestsService.AdminGetAllLeaveRequests();
            return View(model);
        }

        /// <summary>
        /// Loads detailed information for a single leave request so an administrator can review the details and decide whether to approve or decline it.
        /// </summary>
        /// <param name="id"></param>
        public async Task<IActionResult> Review(int id)
        {
            var model = await _leaveRequestsService.GetLeaveRequestForReview(id);
            return View(model);
        }

        /// <summary>
        /// Processes the administrator’s decision to approve or decline a leave request and updates its status accordingly.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="approved"></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Review(int id, bool approved)
        {
            await _leaveRequestsService.ReviewLeaveRequest(id, approved);
            return RedirectToAction(nameof(ListRequests));
        }
    }
}
