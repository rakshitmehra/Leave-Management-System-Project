using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Web.Models.LeaveTypes
{
    /// <summary>
    /// Read-only view model for displaying leave type information, including its name and maximum allowed days.
    /// </summary>

    public class LeaveTypeReadOnlyVM : BaseLeaveTypeVM
    {
        public string Name { get; set; } = string.Empty;
        
        [Display(Name = "Maximum Allocation of Days")] 
        public int NumberOfDays { get; set; }
    }
}
