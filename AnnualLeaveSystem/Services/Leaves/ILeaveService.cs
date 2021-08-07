namespace AnnualLeaveSystem.Services.Leaves
{
    using System;
    using System.Collections.Generic;
    using AnnualLeaveSystem.Data.Models;
    using AnnualLeaveSystem.Models.Leaves;

    public interface ILeaveService
    {
        public bool Edit(
           int leaveId,
           DateTime startDate,
           DateTime endDate,
           int totalDays,
           int leaveTypeId,
           string requestEmployeeId,
           string substituteEmployeeId,
           string approveEmployeeId,
           string comments);

        public int Create(
            DateTime startDate,
            DateTime endDate,
            int totalDays,
            int leaveTypeId,
            string requestEmployeeId,
            string substituteEmployeeId,
            string approveEmployeeId,
            string comments,
            DateTime requestDate);

        LeaveQueryServiceModel All(
            Status? status,
            string firstName,
            string lastName,
            LeaveSorting sorting,
            int currentPage,
            int leavesPerPage,
            bool isTeamLead,
            string employeeId);

        public IEnumerable<LeaveServiceModel> ByEmployee(string employeeId);

        public ICollection<SubstituteEmployeeServiceModel> GetEmployeesInTeam(string currentEmployeeId);

        public IEnumerable<LeaveTypeServiceModel> GetLeaveTypes();

        public IEnumerable<OfficialHoliday> GetHolidays();

        public EditLeaveServiceModel GetLeave(int leaveId);

        public int GetLeaveTypeId(int leaveId);

        public int GetLeaveTotalDays(int leaveId);

        public LeaveDetailsServiceModel GetLeaveById(int leaveId);

        public IEnumerable<LeaveServiceModel> LeavesForApproval(string employeeId, bool isTeamLead);

        public void Approve(int leaveId, bool isUser);

        public void Cancel(int leaveId);

        public void Reject(int leaveId);

        public IEnumerable<DateValidationServiceModel> GetNotFinishedLeaves(string employeeId);

        public IEnumerable<DateValidationServiceModel> GetSubstituteApprovedLeaves(string substituteId);

        public bool Exist(int leaveId);

        public bool IsOwn(int leaveId, string employeeId);
    }
}
