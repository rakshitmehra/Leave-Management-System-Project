namespace LeaveManagementSystem.Application.Models.LeaveTypes
{
    /// <summary>
    /// View model used when creating a new leave type, including the name and maximum number of days allowed.
    /// </summary>

    public class LeaveTypeCreateVM
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
