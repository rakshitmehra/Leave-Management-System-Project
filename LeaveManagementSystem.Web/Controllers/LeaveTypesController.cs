using LeaveManagementSystem.Application.Models.LeaveTypes;
using LeaveManagementSystem.Application.Services.LeaveTypes;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Controllers
{
    /// <summary>
    /// Controller responsible for managing Leave Types in the system.
    /// Provides full CRUD functionality: listing, viewing details, creating, editing, and deleting leave types.
    /// Uses a service layer instead of direct database access, ensuring cleaner and more maintainable code.
    /// Only administrators can access these actions.
    /// Includes custom validation to prevent duplicate leave type names.
    /// </summary>

    [Authorize(Roles = Roles.Administrator)]
    public class LeaveTypesController(ILeaveTypesService _leaveTypesService) : Controller
    {
        private const string NameExistsValidationMessage = "This leave type already exists in the database";

        /// <summary>
        /// Retrieves all leave types from the service and displays them in the index view. (GET: LeaveTypes)
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var viewData = await _leaveTypesService.GetAll();
            return View(viewData);
        }

        /// <summary>
        /// GET: LeaveTypes/Details/5
        /// Displays detailed information for a specific leave type. Returns NotFound if the ID is missing or the leave type cannot be found.
        /// </summary>
        /// <param name="id"></param>

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var leaveType = await _leaveTypesService.Get<LeaveTypeReadOnlyVM>(id.Value);
            if (leaveType == null)
            {
                return NotFound();
            }
            return View(leaveType);
        }

        /// <summary>
        /// Displays the form for creating a new leave type. (GET: LeaveTypes/Create)
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Creates a new leave type after validating the model.
        /// Checks if a leave type with the same name already exists and adds a model error if so.
        /// If validation succeeds, saves the new leave type and redirects to the index page.
        /// POST: LeaveTypes/Create
        /// </summary>
        /// <param name="leaveTypeCreate"></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveTypeCreateVM leaveTypeCreate)
        {
            // Adding custom validation and model state error
            if (await _leaveTypesService.CheckIfLeaveTypeNameExists(leaveTypeCreate.Name))
            {
                ModelState.AddModelError(nameof(leaveTypeCreate.Name), "This leave type already exists in the database");
            }

            if (ModelState.IsValid)
            {
                await _leaveTypesService.Create(leaveTypeCreate);
                return RedirectToAction(nameof(Index));
            }
            return View(leaveTypeCreate);
        }

        /// <summary>
        /// Retrieves an existing leave type for editing.
        /// Returns NotFound if the ID is missing or the leave type cannot be found. (GET: LeaveTypes/Edit/5)
        /// </summary>
        /// <param name="id"></param>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveType = await _leaveTypesService.Get<LeaveTypeEditVM>(id.Value);
            if (leaveType == null)
            {
                return NotFound();
            }
            return View(leaveType);
        }


        /// <summary>
        /// Updates an existing leave type after validating the model and ensuring the ID matches.
        /// Verifies that the edited name does not duplicate an existing leave type's name.
        /// Attempts to save changes and handles concurrency exceptions.
        /// Redirects to the index page if successful.
        /// POST: LeaveTypes/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="leaveTypeEdit"></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeaveTypeEditVM leaveTypeEdit)
        {
            if (id != leaveTypeEdit.Id)
            {
                return NotFound();
            }

            if (await _leaveTypesService.CheckIfLeaveTypeNameExistsForEdit(leaveTypeEdit))
            {
                ModelState.AddModelError(nameof(leaveTypeEdit.Name), NameExistsValidationMessage);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _leaveTypesService.Edit(leaveTypeEdit);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_leaveTypesService.LeaveTypeExists(leaveTypeEdit.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(leaveTypeEdit);
        }

        /// <summary>
        /// Retrieves a leave type for deletion confirmation.
        /// Returns NotFound if the ID is missing or the leave type cannot be found.
        /// GET: LeaveTypes/Delete/5
        /// </summary>
        /// <param name="id"></param>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveType = await _leaveTypesService.Get<LeaveTypeReadOnlyVM>(id.Value);
            if (leaveType == null)
            {
                return NotFound();
            }
            return View(leaveType);
        }

        /// <summary>
        /// Permanently deletes the specified leave type and redirects back to the index page.
        /// POST: LeaveTypes/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _leaveTypesService.Remove(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
