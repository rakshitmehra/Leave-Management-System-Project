namespace LeaveManagementSystem.Web.Data
{
    /// <summary>
    /// Defines a leave request status(e.g., Pending, Approved, Rejected, Canceled).
    /// </summary>
    public class LeaveRequestStatus : BaseEntity
    {
        [StringLength(50)]
        public string Name { get; set; }
    }
}