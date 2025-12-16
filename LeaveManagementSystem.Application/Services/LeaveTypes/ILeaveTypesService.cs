using LeaveManagementSystem.Application.Models.LeaveTypes;

namespace LeaveManagementSystem.Application.Services.LeaveTypes
{
    /// <summary>
    /// Validates whether the number of requested days exceeds the maximum allowed for the specified leave type.
    /// </summary>
    public interface ILeaveTypesService
    {
        Task<bool> CheckIfLeaveTypeNameExists(string name);
        Task<bool> CheckIfLeaveTypeNameExistsForEdit(LeaveTypeEditVM leaveTypeEdit);
        Task Create(LeaveTypeCreateVM model);
        Task<bool> DaysExceedMaximum(int leaveTypeId, int days);
        Task Edit(LeaveTypeEditVM model);
        Task<T?> Get<T>(int id) where T : class;
        Task<List<LeaveTypeReadOnlyVM>> GetAll();
        bool LeaveTypeExists(int id);
        Task Remove(int id);
    }
}