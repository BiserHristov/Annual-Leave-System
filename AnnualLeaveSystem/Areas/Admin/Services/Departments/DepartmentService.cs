using AnnualLeaveSystem.Data;
using AnnualLeaveSystem.Services.Users;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using System.Linq;

namespace AnnualLeaveSystem.Areas.Admin.Services.Departments
{
    public class DepartmentService : IDepartmentService
    {
        private readonly LeaveSystemDbContext db;
        private readonly IConfigurationProvider mapper;

        public DepartmentService(LeaveSystemDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper.ConfigurationProvider;
        }


        public IEnumerable<RegisterDepartamentServiceModel> All()
        {
            return this.db.Departments
                .ProjectTo<RegisterDepartamentServiceModel>(mapper)
                .ToList();
        }
    }
}