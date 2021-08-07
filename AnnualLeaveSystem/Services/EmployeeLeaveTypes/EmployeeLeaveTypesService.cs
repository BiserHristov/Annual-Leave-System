namespace AnnualLeaveSystem.Services.EmployeeLeaveTypes
{
    using System.Linq;
    using AnnualLeaveSystem.Data;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;

    public class EmployeeLeaveTypesService : IEmployeeLeaveTypesService
    {
        private readonly LeaveSystemDbContext db;
        private readonly IConfigurationProvider mapper;

        public EmployeeLeaveTypesService(LeaveSystemDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper.ConfigurationProvider;
        }

        public EmployeeLeaveTypesServiceModel GetLeaveType(string employeeId, int leaveTypeId)
        {
            return this.db.EmployeesLeaveTypes
              .Include(x => x.LeaveType)
              .Where(el => el.EmployeeId == employeeId &&
                           el.LeaveTypeId == leaveTypeId)
              .ProjectTo<EmployeeLeaveTypesServiceModel>(this.mapper)
              .FirstOrDefault();
        }
    }
}
