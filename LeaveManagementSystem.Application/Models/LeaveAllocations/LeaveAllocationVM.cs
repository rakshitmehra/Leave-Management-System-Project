using LeaveManagementSystem.Application.Models.LeaveTypes;
using LeaveManagementSystem.Application.Models.Periods;

namespace LeaveManagementSystem.Application.Models.LeaveAllocations
{
    /// <summary>
    /// View model representing a single leave allocation, including the number of days assigned, the period it applies to, and the related leave type.
    /// </summary>

    public class LeaveAllocationVM
    {
        public int Id { get; set; }

        [Display(Name = "Number Of Days")]
        public int Days { get; set; }

        [Display(Name = "Allocation Period")]
        public PeriodVM Period { get; set; } = new PeriodVM();

        public LeaveTypeReadOnlyVM LeaveType { get; set; } = new LeaveTypeReadOnlyVM();
    }
}