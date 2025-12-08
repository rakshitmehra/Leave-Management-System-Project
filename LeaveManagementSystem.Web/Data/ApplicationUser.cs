using Microsoft.AspNetCore.Identity;

namespace LeaveManagementSystem.Web.Data
{
    /// <summary>
    /// Extends IdentityUser with extra profile fields such as first name,last name, and date of birth.
    /// </summary>

    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
