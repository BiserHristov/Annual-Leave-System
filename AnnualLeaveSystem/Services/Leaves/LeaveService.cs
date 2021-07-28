namespace AnnualLeaveSystem.Services.Leaves
{
    using AnnualLeaveSystem.Data;
    using AnnualLeaveSystem.Data.Models;
    using AnnualLeaveSystem.Models.Leaves;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using static AnnualLeaveSystem.Data.DataConstants.User;

    public class LeaveService : ILeaveService
    {
        private readonly LeaveSystemDbContext db;

        public LeaveService(LeaveSystemDbContext db)
        {
            this.db = db;
        }

        public LeaveQueryServiceModel All(
            Status? status,
            string firstName,
            string lastName,
            LeaveSorting sorting,
            int currentPage,
            int leavesPerPage)
        {
            var leavesQuery = this.db.Leaves.AsQueryable();

            if (status != null)
            {
                switch (status)
                {

                    case Status.Approved:
                        leavesQuery = leavesQuery.Where(l => l.LeaveStatus == Status.Approved);
                        break;

                    case Status.Pending:
                        leavesQuery = leavesQuery.Where(l => l.LeaveStatus == Status.Pending);
                        break;

                    case Status.Canceled:
                        leavesQuery = leavesQuery.Where(l => l.LeaveStatus == Status.Canceled);
                        break;

                    case Status.Rejected:
                        leavesQuery = leavesQuery.Where(l => l.LeaveStatus == Status.Rejected);
                        break;

                        //Default case is when "query.Status=null". In that case we do not need any filter by status.
                }

            }


            if (!string.IsNullOrWhiteSpace(firstName))
            {
                leavesQuery = leavesQuery.Where(l => l.RequestEmployee.FirstName.ToLower().Contains(firstName.Trim().ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(lastName))
            {
                leavesQuery = leavesQuery.Where(l => l.RequestEmployee.LastName.ToLower() == lastName.Trim().ToLower());
            }


            switch (sorting)
            {

                case LeaveSorting.RequestDate:
                    leavesQuery = leavesQuery.OrderByDescending(x => x.RequestDate);
                    break;

                case LeaveSorting.TotalDays:
                    leavesQuery = leavesQuery.OrderByDescending(x => x.TotalDays);
                    break;

                case LeaveSorting.Fullname:
                    leavesQuery = leavesQuery
                        .OrderBy(x => x.RequestEmployee.FirstName)
                        .ThenBy(x => x.RequestEmployee.LastName);
                    break;

                case LeaveSorting.StartDate:
                default:
                    leavesQuery = leavesQuery.OrderByDescending(x => x.StartDate);
                    break;

            }

            var totalLeaves = leavesQuery.Count();

            leavesQuery = leavesQuery
                .Skip((currentPage - 1) * leavesPerPage)
                .Take(leavesPerPage);

            var leaves = GetLeaves(leavesQuery);


            return new LeaveQueryServiceModel
            {
                FirstName = firstName,
                LastName = lastName,
                Sorting = sorting,
                Status = status,
                CurrentPage = currentPage,
                LeavesPerPage = leavesPerPage,
                TotalLeaves = totalLeaves,
                Leaves = leaves
            };
        }

        public IEnumerable<LeaveServiceModel> ByEmployee(string employeeId)
        {
            var resultQuery = this.db.Leaves
                 .Where(e => e.RequestEmployeeId == employeeId);

            return GetLeaves(resultQuery);
        }

        private IEnumerable<LeaveServiceModel> GetLeaves(IQueryable<Leave> query)
        {
            return query
                .Select(l => new LeaveServiceModel
                {
                    Id = l.Id,
                    RequestEmployeeId = l.RequestEmployeeId,
                    FirstName = l.RequestEmployee.FirstName,
                    LastName = l.RequestEmployee.LastName,
                    StartDate = l.StartDate.ToLocalTime().Date,
                    EndDate = l.EndDate.ToLocalTime().Date,
                    TotalDays = l.TotalDays,
                    ApprovedBySubstitute = l.ApprovedBySubstitute,
                    Status = l.LeaveStatus.ToString(),
                    RequestDate = l.RequestDate.ToLocalTime().Date,
                })
                .ToList();
        }

        public IEnumerable<SubstituteEmployeeServiceModel> GetEmployeesInTeam(string currentEmployeeId)
        {

            var currentEmployeeTeamId = this.db.Employees
                .Where(e => e.Id == currentEmployeeId)
                .Select(e => e.TeamId)
                .FirstOrDefault();

            return this.db.Employees
              .Where(e => e.TeamId == currentEmployeeTeamId && e.Id != currentEmployeeId)
              .Select(e => new SubstituteEmployeeServiceModel
              {
                  Id = e.Id,
                  Name = $"{e.FirstName} {e.MiddleName} {e.LastName}",
              })
              .ToList();
        }


        public IEnumerable<LeaveTypeServiceModel> GetLeaveTypes()
        {

            return this.db.LeaveTypes
                 .Select(l => new LeaveTypeServiceModel
                 {
                     Id = l.Id,
                     Name = l.Name
                 })
                 .ToList();
        }
        public EditLeaveServiceModel GetLeave(int leaveId)
        {


            return this.db.Leaves
                .Where(l => l.Id == leaveId)
                .Select(l => new EditLeaveServiceModel
                {
                    StartDate = l.StartDate,
                    EndDate = l.EndDate,
                    TotalDays = l.TotalDays,
                    RequestEmployeeId = l.RequestEmployeeId,
                    SubstituteEmployeeId = l.SubstituteEmployeeId,
                    ApprovedBySubstitute = l.ApprovedBySubstitute,
                    ApproveEmployeeId = l.ApproveEmployeeId,
                    LeaveTypeId = l.LeaveTypeId,
                    Comments = l.Comments,

                })
                .FirstOrDefault();
        }

        public IEnumerable<LeaveServiceModel> LeavesForApproval(string employeeId)
        {
            var leaves = this.db.Leaves
                .Where(l => l.SubstituteEmployeeId == employeeId &&
                            l.ApprovedBySubstitute == false)
                .Select(l => new LeaveServiceModel
                {
                    Id = l.Id,
                    FirstName = l.RequestEmployee.FirstName,
                    LastName = l.RequestEmployee.LastName,
                    StartDate = l.StartDate,
                    EndDate = l.EndDate,
                    TotalDays = l.TotalDays,
                    RequestDate = l.RequestDate.ToLocalTime().Date,
                    ApprovedBySubstitute = l.ApprovedBySubstitute,
                    RequestEmployeeId = l.RequestEmployeeId,
                    SubstituteEmployeeId = l.SubstituteEmployeeId,
                    Status = l.LeaveStatus.ToString()
                })
                .ToList();

            return leaves;
        }

        public IEnumerable<DateValidationServiceModel> GetNotFinishedLeaves(string employeeId)
        {
            return this.db.Leaves
                .Where(l => l.RequestEmployeeId == employeeId && l.EndDate >= DateTime.UtcNow.Date)
                .Select(l => new DateValidationServiceModel
                {
                    StartDate = l.StartDate,
                    EndDate = l.EndDate
                })
                .ToList();
        }

        public IEnumerable<DateValidationServiceModel> GetSubstituteApprovedLeaves(string employeeId)
        {
            return this.db.Leaves
               .Where(l => l.SubstituteEmployeeId == employeeId &&
                           l.LeaveStatus == Status.Approved &&
                           l.EndDate >= DateTime.UtcNow.Date)
               .Select(l => new DateValidationServiceModel
               {
                   StartDate = l.StartDate,
                   EndDate = l.EndDate
               })
               .ToList();
        }

        public IEnumerable<OfficialHoliday> GetHolidays()
        {
            return this.db.OfficialHolidays.ToList();
        }


    }
}
