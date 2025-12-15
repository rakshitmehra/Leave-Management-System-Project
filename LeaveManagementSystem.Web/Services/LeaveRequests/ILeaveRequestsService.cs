using LeaveManagementSystem.Web.Models.LeaveRequests;

namespace LeaveManagementSystem.Web.Services.LeaveRequests
{
    /// <summary>
    /// Provides an abstraction for managing leave request operations, including creating requests, retrieving employee and admin leave request data, validating requested dates, canceling requests, and reviewing/approving or declining them.
    /// </summary>
    public interface ILeaveRequestsService
    {
        Task CreateLeaveRequest(LeaveRequestCreateVM model);
        Task<List<LeaveRequestReadOnlyVM>> GetEmployeeLeaveRequests();
        Task<EmployeeLeaveRequestListVM> AdminGetAllLeaveRequests();
        Task CancelLeaveRequest(int leaveRequestId);
        Task ReviewLeaveRequest(int leaveRequestId, bool approved);
        Task <bool> RequestDatesExceedAllocation (LeaveRequestCreateVM model);
        Task<ReviewLeaveRequestVM> GetLeaveRequestForReview(int id);
    }
}