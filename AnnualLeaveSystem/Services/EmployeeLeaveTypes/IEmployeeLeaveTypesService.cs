namespace AnnualLeaveSystem.Services.EmployeeLeaveTypes
{
    public interface IEmployeeLeaveTypesService
    {
        public EmployeeLeaveTypesServiceModel GetLeaveType(string employeeId, int leaveTypeId);
    }
}
