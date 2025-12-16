namespace LeaveManagementSystem.Application.Models.LeaveAllocations
{
    /// <summary>
    /// View model that extends basic employee details to include date of birth,
    /// leave allocation status, and a list of the employee’s leave allocations.
    /// </summary>
    public class EmployeeAllocationVM : EmployeeListVM
    {
        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }
        public bool IsCompletedAllocation { get; set; }
        public List<LeaveAllocationVM> LeaveAllocations { get; set; }
    }
}
