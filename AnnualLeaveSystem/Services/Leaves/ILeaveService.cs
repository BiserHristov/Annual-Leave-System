namespace AnnualLeaveSystem.Services.Leaves
{
    using AnnualLeaveSystem.Data.Models;
    using AnnualLeaveSystem.Models.Leaves;
    using System.Collections.Generic;

    public interface ILeaveService
    {
        LeaveQueryServiceModel All(
            Status? status,
            string firstName,
            string lastName,
            LeaveSorting sorting,
            int currentPage,
            int leavesPerPage);

        public IEnumerable<LeaveServiceModel> ByEmployee(string employeeId);

        public IEnumerable<SubstituteEmployeeServiceModel> GetEmployeesInTeam(string currentEmployeeId);

        public IEnumerable<LeaveTypeServiceModel> GetLeaveTypes();
        public IEnumerable<OfficialHoliday> GetHolidays();
        public EditLeaveServiceModel GetLeave(int leaveId);
        public IEnumerable<LeaveServiceModel> LeavesForApproval(string employeeId);
        public IEnumerable<DateValidationServiceModel> GetNotFinishedLeaves(string employeeId);
        public IEnumerable<DateValidationServiceModel> GetSubstituteApprovedLeaves(string substituteId);

    }
}
