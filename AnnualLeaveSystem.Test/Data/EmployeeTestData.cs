namespace AnnualLeaveSystem.Test.Data
{
    using System;
    using AnnualLeaveSystem.Data.Models;
    using AnnualLeaveSystem.Test.Mocks;

    public static class EmployeeTestData
    {
        public static string UserId => "1b99c696-64f5-443a-ae1e-6b4a1bc8f2cb";

        public static string SubstituteId => "1b99c696-64f5-443a-ae1e-gst5wuuw6112";

        public static Employee Create()
        {
            var data = DatabaseMock.Instance;

            var department = new Department();
            data.Departments.Add(department);
            data.SaveChanges();

            var team = new Team();
            data.Teams.Add(team);
            data.SaveChanges();

            var employee = new Employee
            {
                Id = "1b99c696-64f5-443a-ae1e-6b4a1bc8f2cb",
                UserName = "testUername@abv.bg",
                Email = "testUername@abv.bg",
                PasswordHash = "sdasd324olkk34dff",
                FirstName = "TestFName",
                LastName = "TestLName",
                ImageUrl = "https://thumbs.dreamstime.com/z/artificial-nose-3655155.jpg",
                JobTitle = "TestJobTitle",
                HireDate = new DateTime(2010, 07, 20).ToUniversalTime().Date,
                DepartmentId = 1,
                TeamId = 1,
            };
            var substituteEmployee = new Employee
            {
                Id = "1b99c696-64f5-443a-ae1e-gst5wuuw6112",
                UserName = "testSubstitute@abv.bg",
                Email = "testSubstitute@abv.bg",
                PasswordHash = "sdasd324olkk34dff",
                FirstName = "TestFNameSub",
                LastName = "TestLNameSub",
                ImageUrl = "https://thumbs.dreamstime.com/z/artificial-nose-3655155.jpg",
                JobTitle = "TestJobTitle",
                HireDate = new DateTime(2010, 07, 20).ToUniversalTime().Date,
                DepartmentId = 1,
                TeamId = 1,
            };
            data.Employees.Add(employee);
            data.Employees.Add(substituteEmployee);

            var leaveType = new LeaveType
            {
                Name = "TestType",
                DefaultDays = 10
            };

            data.LeaveTypes.Add(leaveType);

            var employeeLeaveType = new EmployeeLeaveType
            {
                EmployeeId = employee.Id,
                LeaveTypeId = leaveType.Id
            };

            data.EmployeesLeaveTypes.Add(employeeLeaveType);
            data.SaveChanges();

            return employee;
        }
    }
}
