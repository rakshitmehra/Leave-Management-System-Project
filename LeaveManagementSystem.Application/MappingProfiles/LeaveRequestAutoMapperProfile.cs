using AutoMapper;
using LeaveManagementSystem.Application.Models.LeaveRequests;

namespace LeaveManagementSystem.Application.MappingProfiles
{
    /// <summary>
    /// Defines AutoMapper mappings related to leave request operations.
    /// yySpecifically sets up the transformation rules between the LeaveRequestCreateVM view model and the LeaveRequest domain/entity model to support clean and consistent data conversion.
    /// </summary>
    public class LeaveRequestAutoMapperProfile : Profile
    {
        public LeaveRequestAutoMapperProfile()
        {
            CreateMap<LeaveRequestCreateVM, LeaveRequest>();
        }
    }
}
