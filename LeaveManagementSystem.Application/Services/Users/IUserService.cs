namespace LeaveManagementSystem.Application.Services.Users
{
    /// <summary>
    /// Defines a contract for user-related operations, including retrieving all employees, obtaining the currently logged-in user, and fetching a user by their ID.
    /// </summary>
    public interface IUserService
    {
        Task<List<ApplicationUser>> GetEmployees();
        Task<ApplicationUser> GetLoggedInUser();
        Task<ApplicationUser> GetUserById(string userId);
    }
}