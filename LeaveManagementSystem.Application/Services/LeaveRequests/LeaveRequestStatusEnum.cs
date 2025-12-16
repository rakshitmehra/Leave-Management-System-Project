namespace LeaveManagementSystem.Application.Services.LeaveRequests
{
    /// <summary>
    /// Defining the possible statuses of a leave request, representing its workflow state from submission to final outcome.
    /// </summary>
    public enum LeaveRequestStatusEnum
    {
        Pending = 1,
        Approved = 2,
        Declined = 3,
        Canceled = 4,
    }
}
