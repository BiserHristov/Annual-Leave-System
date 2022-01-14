namespace AnnualLeaveSystem.Services.Leaves
{
    using System;
    using System.Collections.Generic;
    using AnnualLeaveSystem.Data.Models;
    using AnnualLeaveSystem.Models.Leaves;

    public interface ILeaveService
    {
        public bool Edit(
           string leaveId,
           DateTime startDate,
           DateTime endDate,
           int totalDays,
           int leaveTypeId,
           string requestEmployeeId,
           string substituteEmployeeId,
           string approveEmployeeId,
           string comments);

        public string Create(
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

        public void Approve(string leaveId, bool isUser);

        public void Cancel(string leaveId);

        public void Reject(string leaveId);

        public int GetLeaveTypeId(string leaveId);

        public int GetLeaveTotalDays(string leaveId);

        public bool Exist(string leaveId);

        public bool IsOwn(string leaveId, string employeeId);

        public IEnumerable<LeaveServiceModel> ByEmployee(string employeeId);

        public ICollection<SubstituteEmployeeServiceModel> GetEmployeesInTeam(string currentEmployeeId);

        public IEnumerable<LeaveTypeServiceModel> GetLeaveTypes();

        public IEnumerable<OfficialHoliday> GetHolidays();

        public EditLeaveServiceModel GetLeave(string leaveId);

        public LeaveDetailsServiceModel GetLeaveById(string leaveId);

        public IEnumerable<LeaveServiceModel> LeavesForApproval(string employeeId, bool isTeamLead);

        public IEnumerable<DateValidationServiceModel> GetNotFinishedLeaves(string employeeId);

        public IEnumerable<DateValidationServiceModel> GetSubstituteApprovedLeaves(string substituteId);
    }
}