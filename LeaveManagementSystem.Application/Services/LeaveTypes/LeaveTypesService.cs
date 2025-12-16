using AutoMapper;
using LeaveManagementSystem.Application.Models.LeaveTypes;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Application.Services.LeaveTypes
{
    /// <summary>
    /// Provides operations for managing leave types, including creating, updating, deleting, and retrieving leave type records.  
    /// Also contains validation helpers such as checking for duplicate names and ensuring allocations do not exceed the maximum allowed days.
    /// </summary>

    public class LeaveTypesService(ApplicationDbContext _context, IMapper _mapper) : ILeaveTypesService
    {
        /// <summary>
        /// Returns all leave types from the database and maps them to read-only view models.
        /// </summary>
        public async Task<List<LeaveTypeReadOnlyVM>> GetAll()
        {
            var data = await _context.LeaveTypes.ToListAsync();

            var viewData = _mapper.Map<List<LeaveTypeReadOnlyVM>>(data);

            return viewData;
        }

        /// <summary>
        /// Fetches a leave type by its ID and maps it to a specified view model type. Returns null if not found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        public async Task<T?> Get<T>(int id) where T : class
        {
            var data = await _context.LeaveTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                return null;
            }

            var viewData = _mapper.Map<T>(data);
            return viewData;
        }

        /// <summary>
        /// Deletes a leave type from the database if it exists.
        /// </summary>
        /// <param name="id"></param>
        public async Task Remove(int id)
        {
            var data = await _context.LeaveTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                _context.Remove(data);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Updates an existing leave type using data from an edit view model.
        /// </summary>
        /// <param name="model"></param>
        public async Task Edit(LeaveTypeEditVM model)
        {
            var leaveType = _mapper.Map<LeaveType>(model);
            _context.Update(leaveType);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Creates and saves a new leave type based on the provided view model.
        /// </summary>
        /// <param name="model"></param>
        public async Task Create(LeaveTypeCreateVM model)
        {
            var leaveType = _mapper.Map<LeaveType>(model);
            _context.Add(leaveType);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Checks whether a leave type with the given ID exists.
        /// </summary>
        /// <param name="id"></param>
        public bool LeaveTypeExists(int id)
        {
            return _context.LeaveTypes.Any(e => e.Id == id);
        }

        /// <summary>
        /// Checks if a leave type name already exists (case-insensitive).
        /// </summary>
        /// <param name="name"></param>
        public async Task<bool> CheckIfLeaveTypeNameExists(string name)
        {
            var lowercaseName = name.ToLower();
            return await _context.LeaveTypes.AnyAsync(q => q.Name.ToLower().Equals(lowercaseName));
        }

        /// <summary>
        /// Checks if another leave type already uses the same name during an edit (case-insensitive and excludes current record).
        /// </summary>
        /// <param name="leaveTypeEdit"></param>
        public async Task<bool> CheckIfLeaveTypeNameExistsForEdit(LeaveTypeEditVM leaveTypeEdit)
        {
            var lowercaseName = leaveTypeEdit.Name.ToLower();
            return await _context.LeaveTypes.AnyAsync(q => q.Name.ToLower().Equals(lowercaseName)
                && q.Id != leaveTypeEdit.Id);
        }

        /// <summary>
        /// Validates whether the number of requested days exceeds the maximum allowed for the specified leave type.
        /// </summary>
        /// <param name="leaveTypeId"></param>
        /// <param name="days"></param>
        public async Task<bool> DaysExceedMaximum(int leaveTypeId, int days)
        {
            var leaveType = await _context.LeaveTypes.FindAsync(leaveTypeId);
            return leaveType.NumberOfDays < days;
        }
    }
}
