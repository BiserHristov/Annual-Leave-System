namespace AnnualLeaveSystem.Areas.Admin.Services.Departments
{
    using System.Collections.Generic;
    using System.Linq;
    using AnnualLeaveSystem.Data;
    using AnnualLeaveSystem.Services.Users;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;

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
            => this.db.Departments
                .ProjectTo<RegisterDepartamentServiceModel>(this.mapper)
                .ToList();
    }
}