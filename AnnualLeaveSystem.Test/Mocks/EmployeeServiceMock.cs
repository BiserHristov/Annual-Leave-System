namespace AnnualLeaveSystem.Test.Mocks
{
    using AnnualLeaveSystem.Services.Employees;
    using AnnualLeaveSystem.Test.Data;
    using Moq;

    public static class EmployeeServiceMock
    {
        public static IEmployeeService Instance(string id)
        {
            var employeeServiceMock = new Mock<IEmployeeService>();

            employeeServiceMock
                .Setup(e => e.Get(id))
                .Returns(EmployeeTestData.Create());

            return employeeServiceMock.Object;
        }
    }
}
