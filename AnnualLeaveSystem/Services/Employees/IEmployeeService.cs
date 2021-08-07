namespace AnnualLeaveSystem.Services.Employees
{
    public interface IEmployeeService
    {
        public string TeamLeadId(string employeeId);

        public int? TeamId(string employeeId);

        public bool Exist(string employeeId);
    }
}
