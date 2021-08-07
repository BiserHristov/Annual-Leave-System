namespace AnnualLeaveSystem.Areas.Admin.Services.Employees
{
    using AnnualLeaveSystem.Data;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using System.Collections.Generic;
    using System.Linq;

    public class EmployeeService : IEmployeeService
    {
        private readonly LeaveSystemDbContext db;
        private readonly IConfigurationProvider mapper;

        public EmployeeService(LeaveSystemDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper.ConfigurationProvider;
        }

        public IEnumerable<EmployeeServiceModel> AllEmployees()
        {
            return this.db.Employees
                .ProjectTo<EmployeeServiceModel>(this.mapper)
                .ToList();
        }

        public EditEmployeeServiceModel GetEmployee(string employeeId)
        {
            return this.db.Employees
                .Where(e => e.Id == employeeId)
                .ProjectTo<EditEmployeeServiceModel>(this.mapper)
                .FirstOrDefault();
        }

        public bool Edit(EditEmployeeServiceModel model)
        {
            var employee = this.db.Employees.Find(model.Id);

            if (employee == null)
            {
                return false;

            }

            employee.FirstName = model.FirstName;
            employee.MiddleName = model.MiddleName;
            employee.LastName = model.LastName;
            employee.ImageUrl = model.ImageUrl;
            employee.JobTitle = model.JobTitle;
            employee.TeamId = model.TeamId;
            employee.DepartmentId = model.DepartmentId;
            employee.HireDate = model.HireDate;

            this.db.SaveChanges();
            return true;

        }

        public bool Delete(string employeeId)
        {
            var employee = this.db.Employees.Find(employeeId);

            if (employee == null)
            {
                return false;
            }

            this.db.Employees.Remove(employee);
            this.db.SaveChanges();

            return true;
        }
    }
}
