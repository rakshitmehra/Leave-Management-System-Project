namespace LeaveManagementSystem.Web.Services.Periods
{
    /// <summary>
    /// Defines a contract for retrieving the current active period.
    /// </summary>
    public interface IPeriodsService
    {
        Task<Period> GetCurrentPeriod();
    }
}