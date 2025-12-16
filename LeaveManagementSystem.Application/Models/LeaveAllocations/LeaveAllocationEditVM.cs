namespace LeaveManagementSystem.Application.Models.LeaveAllocations
{
    /// <summary>
    /// View model used for editing a leave allocation, including basic allocation details and the employee it belongs to.
    /// </summary>
    public class LeaveAllocationEditVM : LeaveAllocationVM
    {
        public EmployeeListVM? Employee { get; set; }
    }
}