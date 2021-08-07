namespace AnnualLeaveSystem.Areas.Admin.Services.Departments
{
    using System.Collections.Generic;
    using AnnualLeaveSystem.Services.Users;

    public interface IDepartmentService
    {
        public IEnumerable<RegisterDepartamentServiceModel> All();
    }
}
