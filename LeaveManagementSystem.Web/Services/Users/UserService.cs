using Microsoft.AspNetCore.Identity;

namespace LeaveManagementSystem.Web.Services.Users
{
    /// <summary>
    /// Handles user-related operations by interacting with ASP.NET Identity. Provides methods to retrieve the currently logged-in user, fetch a user by ID, and list all employees based on role membership.
    /// </summary>
    /// <param name="_userManager"></param>
    /// <param name="_httpContextAccessor"></param>
    public class UserService(UserManager<ApplicationUser> _userManager, IHttpContextAccessor _httpContextAccessor) : IUserService
    {
        /// <summary>
        /// Retrieves the currently authenticated user from the HTTP context using the UserManager.
        /// </summary>
        public async Task<ApplicationUser> GetLoggedInUser()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            return user;
        }

        /// <summary>
        /// Fetches a specific user by their unique identifier through the UserManager.
        /// </summary>
        /// <param name="userId"></param>
        public async Task<ApplicationUser> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }

        /// <summary>
        /// Returns all users assigned to the Employee role.
        /// </summary>
        public async Task<List<ApplicationUser>> GetEmployees()
        {
            var employees = await _userManager.GetUsersInRoleAsync(Roles.Employee);
            return employees.ToList();
        }
    }
}
