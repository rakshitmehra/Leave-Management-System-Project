namespace LeaveManagementSystem.Application.Models.Periods
{
    public class PeriodVM
    {
        /// <summary>
        /// View model representing a period, including its name, start date, and end date.
        /// </summary>

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}