namespace LeaveManagementSystem.Web.Data
{
    /// <summary>
    /// Represents a leave allocation for an employee, linking a leave type, a period, and the number of allocated days.
    /// </summary>

    public class LeaveAllocation : BaseEntity
    {
        public LeaveType? LeaveType { get; set; }
        public int LeaveTypeId { get; set; }

        public ApplicationUser? Employee { get; set; }
        public string EmployeeId { get; set; }

        public Period? Period { get; set; }
        public int PeriodId { get; set; }

        public int Days { get; set; }
    }
}
