﻿namespace AnnualLeaveSystem.Infrastructure
{
    using AnnualLeaveSystem.Areas.Admin.Services.Employees;
    using AnnualLeaveSystem.Data.Models;
    using AnnualLeaveSystem.Models.Home;
    using AnnualLeaveSystem.Models.Leaves;
    using AnnualLeaveSystem.Services.Emails;
    using AnnualLeaveSystem.Services.EmployeeLeaveTypes;
    using AnnualLeaveSystem.Services.Holidays;
    using AnnualLeaveSystem.Services.Leaves;
    using AnnualLeaveSystem.Services.Statistics;
    using AnnualLeaveSystem.Services.Users;
    using AutoMapper;

    using static WebConstants;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Department, RegisterDepartamentServiceModel>();

            this.CreateMap<EditLeaveServiceModel, LeaveFormModel>();

            this.CreateMap<Employee, SubstituteEmployeeServiceModel>()
                .ForMember(e => e.Name, cfg => cfg.MapFrom(e => e.FirstName + " " + e.MiddleName + " " + e.LastName));

            this.CreateMap<CommonInfoServiceModel, IndexViewModel>();

            this.CreateMap<LeaveFormModel, EmailServiceModel>()
                .ForMember(x => x.StartDate, cfg => cfg.MapFrom(y => y.StartDate.ToString(DateFormat)))
                .ForMember(x => x.EndDate, cfg => cfg.MapFrom(y => y.EndDate.ToString(DateFormat)));

            this.CreateMap<EmployeeLeaveType, EmployeeLeaveTypesServiceModel>();

            this.CreateMap<Leave, LeaveServiceModel>()
                .ForMember(x => x.FirstName, cfg => cfg.MapFrom(y => y.RequestEmployee.FirstName))
                .ForMember(x => x.LastName, cfg => cfg.MapFrom(y => y.RequestEmployee.LastName))
                .ForMember(x => x.StartDate, cfg => cfg.MapFrom(y => y.StartDate.ToLocalTime().Date))
                .ForMember(x => x.RequestDate, cfg => cfg.MapFrom(y => y.RequestDate.ToLocalTime().Date))
                .ForMember(x => x.EndDate, cfg => cfg.MapFrom(y => y.EndDate.ToLocalTime().Date))
                .ForMember(x => x.Status, cfg => cfg.MapFrom(y => y.LeaveStatus.ToString()));

            this.CreateMap<LeaveType, LeaveTypeServiceModel>();

            this.CreateMap<Leave, EditLeaveServiceModel>();

            this.CreateMap<Leave, DateValidationServiceModel>();

            this.CreateMap<Leave, LeaveDetailsServiceModel>()
                 .ForMember(x => x.RequestEmployeeName, cfg => cfg.MapFrom(y => y.RequestEmployee.FirstName + " " + y.RequestEmployee.MiddleName + " " + y.RequestEmployee.LastName))
                 .ForMember(x => x.Type, cfg => cfg.MapFrom(y => y.LeaveType.Name))
                 .ForMember(x => x.Status, cfg => cfg.MapFrom(y => y.LeaveStatus.ToString()))
                 .ForMember(x => x.ApproveEmployeeName, cfg => cfg.MapFrom(y => y.ApproveEmployee.FirstName + " " + y.ApproveEmployee.MiddleName + " " + y.ApproveEmployee.LastName))
                 .ForMember(x => x.SubstituteEmployeeName, cfg => cfg.MapFrom(y => y.SubstituteEmployee.FirstName + " " + y.SubstituteEmployee.MiddleName + " " + y.SubstituteEmployee.LastName));

            this.CreateMap<Employee, EmployeeServiceModel>()
                .ForMember(x => x.TeamLeadName, cfg => cfg.MapFrom(e => string.IsNullOrEmpty(e.TeamLeadId) ? "-" : e.TeamLead.FirstName + " " + e.TeamLead.LastName));

            this.CreateMap<Employee, EditEmployeeServiceModel>();

            this.CreateMap<OfficialHoliday, HolidayServiceModel>()
                .ForMember(x => x.Date, cfg => cfg.MapFrom(y => y.Date.ToLocalTime().Date.ToString(DateFormat)));
        }
    }
}
