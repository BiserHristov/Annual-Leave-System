namespace AnnualLeaveSystem.Infrastructure
{
    using AnnualLeaveSystem.Data.Models;
    using AnnualLeaveSystem.Models.Leaves;
    using AnnualLeaveSystem.Services.Leaves;
    using AutoMapper;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<EditLeaveServiceModel, LeaveFormModel>();
            this.CreateMap<Employee, SubstituteEmployeeServiceModel>()
                .ForMember(e => e.Name, cfg => cfg.MapFrom(e => e.FirstName + " " + e.MiddleName + " " + e.LastName));
        }
    }
}
