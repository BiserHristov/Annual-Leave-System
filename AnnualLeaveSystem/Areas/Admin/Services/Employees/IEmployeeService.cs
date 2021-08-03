namespace AnnualLeaveSystem.Areas.Admin.Services.Employees
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IEmployeeService
    {
        public IEnumerable<EmployeeServiceModel> AllEmployees();
        public EditEmployeeServiceModel GetEmployee(string employeeId);
        public bool Edit(EditEmployeeServiceModel model);

    }
}
