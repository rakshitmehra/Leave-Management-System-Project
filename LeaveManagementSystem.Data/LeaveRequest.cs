namespace LeaveManagementSystem.Data
{
    /// <summary>
    /// Represents a request for employee leave within the Leave Management System.
    /// Includes details such as leave dates, leave type, request status, the employee submitting the request, the reviewer processing it, and any associated comments.
    /// </summary>
    public class LeaveRequest : BaseEntity
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public LeaveType? LeaveType { get; set; }
        public int LeaveTypeId { get; set; }
        public LeaveRequestStatus? LeaveRequestStatus { get; set; }
        public int LeaveRequestStatusId { get; set; }
        public ApplicationUser? Employee { get; set; }
        public string EmployeeId { get; set; } = default!;
        public ApplicationUser? Reviewer { get; set; }
        public string? ReviewerId { get; set; }
        public string? RequestComments { get; set; }
    }
}