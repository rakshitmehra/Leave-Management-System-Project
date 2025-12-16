namespace LeaveManagementSystem.Data
{
    /// <summary>
    /// Represents a type of leave, including its name and the number of days allowed.
    /// </summary>
    public class LeaveType : BaseEntity
    {
        [MaxLength(150)]
        public string Name { get; set; }
        public int NumberOfDays { get; set; }
        public List<LeaveAllocation>? LeaveAllocations { get; set; }
    }
}
