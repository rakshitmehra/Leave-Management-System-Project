using AutoMapper;
using Humanizer;
using LeaveManagementSystem.Web.Models.LeaveRequests;
using System.Runtime.ConstrainedExecution;

namespace LeaveManagementSystem.Web.MappingProfiles
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
