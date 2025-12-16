namespace LeaveManagementSystem.Application.Models.LeaveTypes
{
    /// <summary>
    /// View model used for editing an existing leave type, including its name and maximum number of days.
    /// </summary>

    public class LeaveTypeEditVM : BaseLeaveTypeVM
    {
        [Required]
        [Length(4, 150, ErrorMessage = "You have violated the length requirements")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(1, 90)]
        [Display(Name = "Maximum Allocation of Days")]
        public int NumberOfDays { get; set; }
    }
}
