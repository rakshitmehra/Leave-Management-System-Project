namespace LeaveManagementSystem.Web.Data
{
    public class Period : BaseEntity
    {
        /// <summary>
        /// Represents a time period with a name, start date, and end date.
        /// </summary>

        public string Name { get; set; }
        public DateOnly StartDate { get;set; }
        public DateOnly EndDate { get;set; }
    }
}
