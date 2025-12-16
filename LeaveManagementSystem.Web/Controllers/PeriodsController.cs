using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Controllers
{
    /// <summary>
    /// Controller responsible for managing Period entities within the system.
    /// This includes listing periods, viewing details, creating new periods, editing existing ones, and deleting them.
    /// All actions are restricted to administrators.
    /// The controller uses Entity Framework Core to access the database and performs all operations asynchronously.
    /// </summary>

    [Authorize(Roles = Roles.Administrator)]
    public class PeriodsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PeriodsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all period records from the database and displays them in the index view. (GET: Periods)
        /// </summary>
        public async Task<IActionResult> Index()
        {
            return View(await _context.Periods.ToListAsync());
        }

        /// <summary>
        /// Shows detailed information for a specific period.
        /// Returns NotFound if the ID is missing or if the period cannot be located in the database.
        /// GET: Periods/Details/5
        /// </summary>
        /// <param name="id"></param>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var period = await _context.Periods
                .FirstOrDefaultAsync(m => m.Id == id);

            if (period == null)
            {
                return NotFound();
            }

            return View(period);
        }

        /// <summary>
        /// Displays the form used to create a new period records. (GET: Periods/Create)
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Creates a new period entry after validating the submitted model.
        /// Saves the period to the database and redirects to the index view if successful.
        /// POST: Periods/Create
        /// </summary>
        /// <param name="period"></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,StartDate,EndDate,Id")] Period period)
        {
            if (ModelState.IsValid)
            {
                _context.Add(period);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(period);
        }

        /// <summary>
        /// Retrieves an existing period by ID and displays it for editing.
        /// Returns NotFound if the ID is missing or if the period does not exist.
        /// GET: Periods/Edit/5
        /// </summary>
        /// <param name="id"></param>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var period = await _context.Periods.FindAsync(id);
            if (period == null)
            {
                return NotFound();
            }
            return View(period);
        }

        /// <summary>
        /// Updates an existing period after validating the model.
        /// Ensures the requested ID matches the period being edited.
        /// Attempts to save changes and handles concurrency exceptions.
        /// Redirects to the index upon successful update.
        /// POST: Periods/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="period"></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,StartDate,EndDate,Id")] Period period)
        {
            if (id != period.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(period);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeriodExists(period.Id))
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
            return View(period);
        }

        /// <summary>
        /// Retrieves a period to confirm deletion.
        /// Returns NotFound if the ID is missing or the period cannot be found.
        /// GET: Periods/Delete/5
        /// </summary>
        /// <param name="id"></param>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var period = await _context.Periods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (period == null)
            {
                return NotFound();
            }

            return View(period);
        }

        /// <summary>
        /// Permanently deletes the specified period from the database and redirects to the index view.
        /// POST: Periods/Delete/5
        /// </summary>
        /// <param name="id"></param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var period = await _context.Periods.FindAsync(id);
            if (period != null)
            {
                _context.Periods.Remove(period);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Checks whether a period with the specified ID exists in the database.
        /// </summary>
        /// <param name="id"></param>
        private bool PeriodExists(int id)
        {
            return _context.Periods.Any(e => e.Id == id);
        }
    }
}
