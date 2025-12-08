namespace LeaveManagementSystem.Web.Models.LeaveAllocations
{
    /// <summary>
    /// View model used to display basic employee information in lists, such as name and email.
    /// </summary>

    public class EmployeeListVM
    {
        public string Id { get; set; } = string.Empty;

        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;
    }
}