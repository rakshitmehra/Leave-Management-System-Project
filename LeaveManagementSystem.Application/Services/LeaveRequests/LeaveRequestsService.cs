using AutoMapper;
using LeaveManagementSystem.Application.Models.LeaveAllocations;
using LeaveManagementSystem.Application.Models.LeaveRequests;
using LeaveManagementSystem.Application.Services.LeaveAllocations;
using LeaveManagementSystem.Application.Services.Users;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Application.Services.LeaveRequests
{
    /// <summary>
    /// Handles all business logic for leave requests, including creating, canceling, reviewing, validating, and retrieving them. Manages leave balances and prepares data models by coordinating with user services, allocation services, AutoMapper, and the database.
    /// </summary>
    /// <param name="_mapper"></param>
    /// <param name="_userService"></param>
    /// <param name="_context"></param>
    /// <param name="_leaveAllocationsService"></param>
    public class LeaveRequestsService(IMapper _mapper, IUserService _userService, ApplicationDbContext _context, ILeaveAllocationsService _leaveAllocationsService) : ILeaveRequestsService
    {
        /// <summary>
        /// Cancels an existing leave request and restores the previously deducted leave days back to the employee’s current allocation for the relevant period.
        /// </summary>
        /// <param name="leaveRequestId"></param>
        public async Task CancelLeaveRequest(int leaveRequestId)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(leaveRequestId);
            leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusEnum.Canceled;

            var currentDate = DateTime.Now;
            var period = await _context.Periods.SingleAsync(q => q.EndDate.Year == currentDate.Year);
            var numberOfDays = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber;

            var allocationToDeduct = await _context.LeaveAllocations
                .FirstAsync(q => q.LeaveTypeId == leaveRequest.LeaveTypeId && q.EmployeeId == leaveRequest.EmployeeId && q.PeriodId == period.Id);

            allocationToDeduct.Days += numberOfDays;

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Creates a new leave request for the logged-in employee, sets it to “Pending,” deducts the requested number of leave days from their current allocation, and saves the request.
        /// </summary>
        /// <param name="model"></param>
        public async Task CreateLeaveRequest(LeaveRequestCreateVM model)
        {
            var leaveRequest = _mapper.Map<LeaveRequest>(model);

            var user = await _userService.GetLoggedInUser();
            leaveRequest.EmployeeId = user.Id;

            leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusEnum.Pending;

            _context.Add(leaveRequest);

            var currentDate = DateTime.Now;
            var period = await _context.Periods.SingleAsync(q => q.EndDate.Year == currentDate.Year);
            var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber;
            var allocationToDeduct = await _context.LeaveAllocations.FirstAsync(q => q.LeaveTypeId == model.LeaveTypeId && q.EmployeeId == user.Id && q.PeriodId == period.Id);

            allocationToDeduct.Days -= numberOfDays;

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves all leave requests in the system, categorizes them by status (approved, pending, declined), and returns a summary model with counts and details for administrative review.
        /// </summary>
        public async Task<EmployeeLeaveRequestListVM> AdminGetAllLeaveRequests()
        {
            var leaveRequests = await _context.LeaveRequests
                .Include(q => q.LeaveType)
                .ToListAsync();

            var approvedLeaveRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Approved);

            var pendingLeaveRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Approved);

            var declinedLeaveRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Approved);

            var leaveRequestModels = leaveRequests.Select(q => new LeaveRequestReadOnlyVM
            {
                StartDate = q.StartDate,
                EndDate = q.EndDate,
                Id = q.Id,
                LeaveType = q.LeaveType.Name,
                LeaveRequestStatus = (LeaveRequestStatusEnum)q.LeaveRequestStatusId,
                NumberOfDays = q.EndDate.DayNumber - q.StartDate.DayNumber
            }).ToList();

            var model = new EmployeeLeaveRequestListVM
            {
                ApprovedRequests = approvedLeaveRequests,
                PendingRequests = pendingLeaveRequests,
                DeclinedRequests = declinedLeaveRequests,
                TotalRequests = leaveRequests.Count,
                LeaveRequests = leaveRequestModels
            };

            return model;
        }

        /// <summary>
        /// Fetches all leave requests belonging to the logged-in employee and returns them as read-only view models.
        /// </summary>
        public async Task<List<LeaveRequestReadOnlyVM>> GetEmployeeLeaveRequests()
        {
            var user = await _userService.GetLoggedInUser();

            var leaveRequests = await _context.LeaveRequests
                .Include(q => q.LeaveType)
                .Where(q => q.EmployeeId == user.Id)
                .ToListAsync();

            var model = leaveRequests.Select(q => new LeaveRequestReadOnlyVM
            {
                StartDate = q.StartDate,
                EndDate = q.EndDate,
                Id = q.Id,
                LeaveType = q.LeaveType.Name,
                LeaveRequestStatus = (LeaveRequestStatusEnum)q.LeaveRequestStatusId,
                NumberOfDays = q.EndDate.DayNumber - q.StartDate.DayNumber
            }).ToList();

            return model;
        }

        /// <summary>
        /// Checks whether the number of days requested exceeds the employee’s remaining allocated leave days for the current period.
        /// </summary>
        /// <param name="model"></param>
        public async Task<bool> RequestDatesExceedAllocation(LeaveRequestCreateVM model)
        {
            var user = await _userService.GetLoggedInUser();

            var currentDate = DateTime.Now;

            var period = await _context.Periods.SingleAsync(q => q.EndDate.Year == currentDate.Year);

            var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber;

            var allocationToDeduct = await _context.LeaveAllocations.FirstAsync(q => q.LeaveTypeId == model.LeaveTypeId && q.EmployeeId == user.Id && q.PeriodId == period.Id);

            return allocationToDeduct.Days < numberOfDays;
        }

        /// <summary>
        /// Approves or declines a leave request, records the reviewer, and restores leave days back to the employee’s allocation if the request is declined.
        /// </summary>
        /// <param name="leaveRequestId"></param>
        /// <param name="approved"></param>
        public async Task ReviewLeaveRequest(int leaveRequestId, bool approved)
        {
            var user = await _userService.GetLoggedInUser();

            var leaveRequest = await _context.LeaveRequests.FindAsync(leaveRequestId);

            leaveRequest.LeaveRequestStatusId = approved ? (int)LeaveRequestStatusEnum.Approved : (int)LeaveRequestStatusEnum.Declined;

            leaveRequest.ReviewerId = user.Id;

            if (!approved)
            {
                var currentDate = DateTime.Now;

                var period = await _context.Periods.SingleAsync(q => q.EndDate.Year == currentDate.Year);

                var allocation = await _context.LeaveAllocations

                .FirstAsync(q => q.LeaveTypeId == leaveRequest.LeaveTypeId && q.EmployeeId == leaveRequest.EmployeeId && q.PeriodId == period.Id);

                var numberOfDays = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber;

                allocation.Days += numberOfDays;
            }

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves a specific leave request with its leave type and employee details and maps them into a review model for managers to assess.
        /// </summary>
        /// <param name="id"></param>
        public async Task<ReviewLeaveRequestVM> GetLeaveRequestForReview(int id)
        {
            var leaveRequest = await _context.LeaveRequests
                .Include(q => q.LeaveType)
                .FirstAsync(q => q.Id == id);

            var user = await _userService.GetUserById(leaveRequest.EmployeeId);

            var model = new ReviewLeaveRequestVM
            {
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                Id = leaveRequest.Id,
                NumberOfDays = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber,
                LeaveRequestStatus = (LeaveRequestStatusEnum)leaveRequest.LeaveRequestStatusId,
                LeaveType = leaveRequest.LeaveType.Name,
                Employee = new EmployeeListVM
                {
                    Id = leaveRequest.EmployeeId,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                }
            };
            return model;
        }

        /// <summary>
        /// Adjusts the employee’s leave allocation by adding or subtracting days based on whether the request is being approved or canceled.
        /// </summary>
        /// <param name="leaveRequest"></param>
        /// <param name="deductDays"></param>
        private async Task UpdateAllocationDays(LeaveRequest leaveRequest, bool deductDays)
        {
            var allocation = await _leaveAllocationsService.GetCurrentAllocation(leaveRequest.LeaveTypeId, leaveRequest.EmployeeId);
            var numberOfDays = CalculateDays(leaveRequest.StartDate, leaveRequest.EndDate);

            if (deductDays)
            {
                allocation.Days -= numberOfDays;
            }
            else
            {
                allocation.Days += numberOfDays;
            }
            _context.Entry(allocation).State = EntityState.Modified;
        }

        /// <summary>
        /// Calculates the total number of days between the start and end dates of a leave request.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private int CalculateDays(DateOnly start, DateOnly end)
        {
            return end.DayNumber - start.DayNumber;
        }
    }
}
