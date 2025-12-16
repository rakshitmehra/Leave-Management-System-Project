
using AutoMapper;
using LeaveManagementSystem.Application.Models.LeaveAllocations;
using LeaveManagementSystem.Application.Services.Periods;
using LeaveManagementSystem.Application.Services.Users;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Application.Services.LeaveAllocations
{
    /// <summary>
    /// Service responsible for managing leave allocations for employees.
    /// It handles creating new allocations, checking existing ones, retrieving employee leave information, updating allocation values, and mapping data between entities and view models.
    /// Uses EF Core for database access and AutoMapper for object mapping.
    /// </summary>

    public class LeaveAllocationsService(ApplicationDbContext _context, IUserService _userService, IMapper _mapper, IPeriodsService _periodsService) : ILeaveAllocationsService
    {
        /// <summary>
        /// Allocates leave entitlements for the specified employee by calculating prorated days based on remaining months in the current period and creating allocation entries for leave types the employee has not yet been assigned.
        /// </summary>
        /// <param name="employeeId"></param>
        public async Task AllocateLeave(string employeeId)
        {
            // get all the leave types
            var leaveTypes = await _context.LeaveTypes
                .Where(q => !q.LeaveAllocations.Any(x => x.EmployeeId == employeeId))
                .ToListAsync();

            // get the current period based on the year
            var period = await _periodsService.GetCurrentPeriod();
            var monthsRemaining = period.EndDate.Month - DateTime.Now.Month;

            foreach (var leaveType in leaveTypes)
            {
                var accuralRate = decimal.Divide(leaveType.NumberOfDays, 12);
                var leaveAllocation = new LeaveAllocation
                {
                    EmployeeId = employeeId,
                    LeaveTypeId = leaveType.Id,
                    PeriodId = period.Id,
                    Days = (int)Math.Ceiling(accuralRate * monthsRemaining)
                };
                _context.Add(leaveAllocation);
            }
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves all leave allocations for a specific employee (or the logged-in user if no ID is provided), maps them to view models, and returns a full employee allocation overview including completed-allocation status.
        /// </summary>
        /// <param name="userId"></param>
        public async Task<EmployeeAllocationVM> GetEmployeeAllocations(string? userId)
        {
            var user = string.IsNullOrEmpty(userId)
                ? await _userService.GetLoggedInUser()
                : await _userService.GetUserById(userId);

            var allocations = await GetAllocations(user.Id);
            var allocationVmList = _mapper.Map<List<LeaveAllocation>, List<LeaveAllocationVM>>(allocations);
            var leaveTypesCount = await _context.LeaveTypes.CountAsync();

            var employeeVm = new EmployeeAllocationVM
            {
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                LeaveAllocations = allocationVmList,
                IsCompletedAllocation = leaveTypesCount == allocations.Count
            };

            return employeeVm;
        }

        /// <summary>
        /// Fetches all employees in the system and maps them into a simplified list view model suitable for display in the UI.
        /// </summary>
        public async Task<List<EmployeeListVM>> GetEmployees()
        {
            var users = await _userService.GetEmployees();
            var employees = _mapper.Map<List<ApplicationUser>, List<EmployeeListVM>>(users.ToList());

            return employees;
        }

        /// <summary>
        /// Retrieves a specific leave allocation by ID, including its related leave type and employee information, and maps it into an editable view model.
        /// </summary>
        /// <param name="allocationId"></param>
        public async Task<LeaveAllocationEditVM> GetEmployeeAllocation(int allocationId)
        {
            var allocation = await _context.LeaveAllocations
                   .Include(q => q.LeaveType)
                   .Include(q => q.Employee)
                   .FirstOrDefaultAsync(q => q.Id == allocationId);

            var model = _mapper.Map<LeaveAllocationEditVM>(allocation);

            return model;
        }

        /// <summary>
        /// Updates the number of allocated leave days for a specific leave allocation entry using an efficient database update operation.
        /// </summary>
        /// <param name="allocationEditVm"></param>
        public async Task EditAllocation(LeaveAllocationEditVM allocationEditVm)
        {
            await _context.LeaveAllocations
                .Where(q => q.Id == allocationEditVm.Id)
                .ExecuteUpdateAsync(s => s.SetProperty(e => e.Days, allocationEditVm.Days));
        }

        /// <summary>
        /// Retrieves the leave allocation for a specific employee and leave type for the current period.
        /// </summary>
        /// <param name="leaveTypeId"></param>
        /// <param name="employeeId"></param>
        public async Task<LeaveAllocation> GetCurrentAllocation(int leaveTypeId, string employeeId)
        {
            var period = await _periodsService.GetCurrentPeriod();
            var allocation = await _context.LeaveAllocations
                    .FirstAsync(q => q.LeaveTypeId == leaveTypeId
                    && q.EmployeeId == employeeId
                    && q.PeriodId == period.Id);
            return allocation;
        }

        /// <summary>
        /// Retrieves all leave allocations for the specified employee for the current period, including their related leave types and period data.
        /// </summary>
        /// <param name="userId"></param>
        private async Task<List<LeaveAllocation>> GetAllocations(string? userId)
        {
            var period = await _periodsService.GetCurrentPeriod();

            var leaveAllocations = await _context.LeaveAllocations
               .Include(q => q.LeaveType)
               .Include(q => q.Period)
               .Where(q => q.EmployeeId == userId && q.Period.Id == period.Id)
               .ToListAsync();
            return leaveAllocations;
        }

        /// <summary>
        /// Checks whether a leave allocation already exists for the specified employee, leave type, and period.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="periodId"></param>
        /// <param name="leaveTypeId"></param>
        private async Task<bool> AllocationExists(string userId, int periodId, int leaveTypeId)
        {
            var exists = await _context.LeaveAllocations.AnyAsync(q =>
                q.EmployeeId == userId
                && q.LeaveTypeId == leaveTypeId
                && q.PeriodId == periodId
            );

            return exists;
        }

    }
}