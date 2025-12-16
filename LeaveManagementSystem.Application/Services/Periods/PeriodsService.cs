using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Application.Services.Periods
{
    /// <summary>
    /// Provides functionality to retrieve the active period based on the current year.
    /// </summary>
    /// <param name="_context"></param>
    public class PeriodsService(ApplicationDbContext _context) : IPeriodsService
    {
        public async Task<Period> GetCurrentPeriod()
        {
            var currentDate = DateTime.Now;
            var period = await _context.Periods.SingleAsync(q => q.EndDate.Year == currentDate.Year);
            return period;
        }
    }
}
