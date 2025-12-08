namespace LeaveManagementSystem.Web.Data
{
    public abstract class BaseEntity
    {
        /// <summary>
        /// A simple base class that provides a shared Id property for all entities.
        /// </summary>
        public int Id { get; set; }
    }
}
