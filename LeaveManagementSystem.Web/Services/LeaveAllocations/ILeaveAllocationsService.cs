using LeaveManagementSystem.Web.Models.LeaveAllocations;

namespace LeaveManagementSystem.Web.Services.LeaveAllocations
{
    /// <summary>
    /// Defines the operations for managing employee leave allocations, including creating allocations, retrieving allocation details, listing employees, and updating existing allocation records.
    /// </summary>

    public interface ILeaveAllocationsService
    {
        Task AllocateLeave(string employeeId);
        Task<EmployeeAllocationVM> GetEmployeeAllocations(string? userId);
        Task<LeaveAllocationEditVM> GetEmployeeAllocation(int allocationId);
        Task<List<EmployeeListVM>> GetEmployees();
        Task EditAllocation(LeaveAllocationEditVM allocationEditVm);
    }
}
