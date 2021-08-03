namespace AnnualLeaveSystem.Areas.Admin.Services.Departments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IDepartmentService
    {
        public IEnumerable<int> All();
    }
}
