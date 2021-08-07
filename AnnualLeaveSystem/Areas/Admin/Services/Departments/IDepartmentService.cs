namespace AnnualLeaveSystem.Areas.Admin.Services.Departments
{
    using AnnualLeaveSystem.Services.Users;
    using System.Collections.Generic;

    public interface IDepartmentService
    {
        public IEnumerable<RegisterDepartamentServiceModel> All();
    }
}
