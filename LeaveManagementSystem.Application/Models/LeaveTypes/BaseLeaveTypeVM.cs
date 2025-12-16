namespace LeaveManagementSystem.Application.Models.LeaveTypes
{
    /// <summary>
    /// Base view model for leave types, containing the shared Id property.
    /// </summary>

    public abstract class BaseLeaveTypeVM
    {
        public int Id { get; set; }
    }
}
