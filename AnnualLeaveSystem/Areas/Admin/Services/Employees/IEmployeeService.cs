namespace AnnualLeaveSystem.Areas.Admin.Services.Employees
{
    using System.Collections.Generic;

    public interface IEmployeeService
    {
        public IEnumerable<EmployeeServiceModel> AllEmployees();
        public EditEmployeeServiceModel GetEmployee(string employeeId);
        public bool Edit(EditEmployeeServiceModel model);
        public bool Delete(string employeeId);


    }
}
