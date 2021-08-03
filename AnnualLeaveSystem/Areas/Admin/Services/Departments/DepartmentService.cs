using AnnualLeaveSystem.Data;
using System.Collections.Generic;
using System.Linq;

namespace AnnualLeaveSystem.Areas.Admin.Services.Departments
{
    public class DepartmentService : IDepartmentService
    {
        private readonly LeaveSystemDbContext db;

        public DepartmentService(LeaveSystemDbContext db)
        {
            this.db = db;
        }


        public IEnumerable<int> All()
        {
            return this.db.Departments
                .Select(d => d.Id)
                .ToList();
        }
    }
}