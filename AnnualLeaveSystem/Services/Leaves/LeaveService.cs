namespace AnnualLeaveSystem.Services.Leaves
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AnnualLeaveSystem.Data;
    using AnnualLeaveSystem.Data.Models;
    using AnnualLeaveSystem.Models.Leaves;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;

    public class LeaveService : ILeaveService
    {
        private readonly LeaveSystemDbContext db;
        private readonly IConfigurationProvider mapper;

        public LeaveService(LeaveSystemDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper.ConfigurationProvider;
        }

        public LeaveQueryServiceModel All(
            Status? status,
            string firstName,
            string lastName,
            LeaveSorting sorting,
            int currentPage,
            int leavesPerPage,
            bool isTeamLead,
            string employeeId)
        {
            var leavesQuery = this.db.Leaves.AsQueryable();

            if (isTeamLead)
            {
                var teamId = this.db.Employees
                    .Where(e => e.Id == employeeId)
                    .Select(e => e.TeamId)
                    .FirstOrDefault();

                leavesQuery = leavesQuery.Where(l => l.RequestEmployee.TeamId == teamId);
            }

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

            var leaves = this.GetLeaves(leavesQuery);

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

            return this.GetLeaves(resultQuery);
        }

        public ICollection<SubstituteEmployeeServiceModel> GetEmployeesInTeam(string currentEmployeeId)
        {
            var currentEmployeeTeamId = this.db.Employees
                .Where(e => e.Id == currentEmployeeId)
                .Select(e => e.TeamId)
                .FirstOrDefault();

            return this.db.Employees
              .Where(e => e.TeamId == currentEmployeeTeamId && e.Id != currentEmployeeId)
              .ProjectTo<SubstituteEmployeeServiceModel>(this.mapper)
              .ToList();
        }

        public IEnumerable<LeaveTypeServiceModel> GetLeaveTypes()
            => this.db.LeaveTypes
                .ProjectTo<LeaveTypeServiceModel>(this.mapper)
                .ToList();

        public EditLeaveServiceModel GetLeave(int leaveId)
            => this.db.Leaves
                .Where(l => l.Id == leaveId)
                .ProjectTo<EditLeaveServiceModel>(this.mapper)
                .FirstOrDefault();

        public IEnumerable<LeaveServiceModel> LeavesForApproval(string employeeId, bool isTeamLead)
        {
            var query = this.db.Leaves
                .Where(l => l.LeaveStatus == Status.Pending)
                .AsQueryable();

            if (isTeamLead)
            {
                var teamLeadTeamId = this.db.Employees
                    .Where(e => e.Id == employeeId)
                    .Select(x => x.TeamId)
                    .FirstOrDefault();
                query = query
                    .Where(l => l.RequestEmployee.TeamId == teamLeadTeamId &&
                                l.ApprovedBySubstitute);
            }
            else
            {
                query = query
                    .Where(l => l.SubstituteEmployeeId == employeeId &&
                                !l.ApprovedBySubstitute);
            }

            return query
                .ProjectTo<LeaveServiceModel>(this.mapper)
                .ToList();
        }

        public void Approve(int leaveId, bool isUser)
        {
            var leave = this.db.Leaves
                .Where(l => l.Id == leaveId)
                .FirstOrDefault();

            if (isUser && leave.RequestEmployeeId != leave.ApproveEmployeeId)
            {
                leave.ApprovedBySubstitute = true;
            }
            else
            {
                leave.LeaveStatus = Status.Approved;

                var employeeLeaveType = this.db.EmployeesLeaveTypes
                    .Where(el => el.EmployeeId == leave.RequestEmployeeId &&
                                 el.LeaveTypeId == leave.LeaveTypeId)
                    .FirstOrDefault();

                employeeLeaveType.PendingApprovalDays -= leave.TotalDays;
                employeeLeaveType.UsedDays += leave.TotalDays;
            }

            this.db.SaveChanges();
        }

        public void Cancel(int leaveId)
            => this.ChangeStatus(leaveId, Status.Canceled);

        public void Reject(int leaveId)
            => this.ChangeStatus(leaveId, Status.Rejected);

        public IEnumerable<DateValidationServiceModel> GetNotFinishedLeaves(string employeeId)
            => this.db.Leaves
                .Where(l => l.RequestEmployeeId == employeeId &&
                            (l.LeaveStatus == Status.Pending || l.LeaveStatus == Status.Approved) &&
                            l.EndDate >= DateTime.UtcNow.Date)
                .ProjectTo<DateValidationServiceModel>(this.mapper)
                .ToList();

        public IEnumerable<DateValidationServiceModel> GetSubstituteApprovedLeaves(string employeeId)
            => this.db.Leaves
               .Where(l => l.SubstituteEmployeeId == employeeId &&
                           l.LeaveStatus == Status.Approved &&
                           l.EndDate >= DateTime.UtcNow.Date)
               .ProjectTo<DateValidationServiceModel>(this.mapper)
               .ToList();

        public IEnumerable<OfficialHoliday> GetHolidays()
            => this.db.OfficialHolidays.ToList();

        public int Create(
            DateTime startDate,
            DateTime endDate,
            int totalDays,
            int leaveTypeId,
            string requestEmployeeId,
            string substituteEmployeeId,
            string approveEmployeeId,
            string comments,
            DateTime requestDate)
        {
            var leave = new Leave
            {
                StartDate = startDate.Date,
                EndDate = endDate.Date,
                TotalDays = totalDays,
                LeaveTypeId = leaveTypeId,
                RequestEmployeeId = requestEmployeeId,
                SubstituteEmployeeId = substituteEmployeeId,
                ApproveEmployeeId = approveEmployeeId ?? requestEmployeeId,
                ApprovedBySubstitute = substituteEmployeeId == approveEmployeeId,
                Comments = comments,
                RequestDate = requestDate
            };

            var employeeLeave = this.db.EmployeesLeaveTypes
              .Include(x => x.LeaveType)
              .Where(el => el.EmployeeId == leave.RequestEmployeeId &&
                           el.LeaveTypeId == leaveTypeId)
              .FirstOrDefault();

            employeeLeave.PendingApprovalDays += totalDays;

            this.db.Leaves.Add(leave);
            this.db.SaveChanges();

            return leave.Id;
        }

        public bool Edit(
            int leaveId,
           DateTime startDate,
           DateTime endDate,
           int totalDays,
           int leaveTypeId,
           string requestEmployeeId,
           string substituteEmployeeId,
           string approveEmployeeId,
           string comments)
        {
            var leave = this.db.Leaves.Find(leaveId);

            if (leave == null)
            {
                return false;
            }

            var employeeLeave = this.GetEmployeeLeaveType(leave.LeaveTypeId, requestEmployeeId);
            employeeLeave.PendingApprovalDays -= leave.TotalDays;

            if (leave.LeaveTypeId == leaveTypeId)
            {
                employeeLeave.PendingApprovalDays += totalDays;
            }
            else
            {
                var currentEmployeeLeave = this.GetEmployeeLeaveType(leaveTypeId, requestEmployeeId);
                currentEmployeeLeave.PendingApprovalDays += totalDays;
            }

            leave.StartDate = startDate.Date;
            leave.EndDate = endDate.Date;
            leave.TotalDays = totalDays;
            leave.LeaveTypeId = leaveTypeId;
            leave.SubstituteEmployeeId = substituteEmployeeId;
            leave.Comments = comments;

            this.db.SaveChanges();

            return true;
        }

        public bool Exist(int leaveId)
            => this.db.Leaves.Any(l => l.Id == leaveId);

        public int GetLeaveTypeId(int leaveId)
            => this.db.Leaves
                .Where(l => l.Id == leaveId)
                .Select(l => l.LeaveTypeId)
                .FirstOrDefault();

        public int GetLeaveTotalDays(int leaveId)
            => this.db.Leaves
             .Where(l => l.Id == leaveId)
             .Select(l => l.TotalDays)
             .FirstOrDefault();

        public bool IsOwn(int leaveId, string employeeId)
            => this.db.Leaves
                .Any(l => l.Id == leaveId && l.RequestEmployeeId == employeeId);

        public LeaveDetailsServiceModel GetLeaveById(int leaveId)
            => this.db.Leaves
                 .Include(l => l.LeaveType)
                 .Include(l => l.RequestEmployee)
                 .Include(l => l.ApproveEmployee)
                 .Include(l => l.SubstituteEmployee)
                 .Where(l => l.Id == leaveId)
                 .ProjectTo<LeaveDetailsServiceModel>(this.mapper)
                 .FirstOrDefault();

        private void ChangeStatus(int leaveId, Status status)
        {
            var leave = this.db.Leaves
                .Where(l => l.Id == leaveId)
                .FirstOrDefault();

            leave.LeaveStatus = status;

            var employeeLeaveType = this.db.EmployeesLeaveTypes
                    .Where(el => el.EmployeeId == leave.RequestEmployeeId &&
                                el.LeaveTypeId == leave.LeaveTypeId)
                    .FirstOrDefault();
            employeeLeaveType.PendingApprovalDays -= leave.TotalDays;

            this.db.SaveChanges();
        }

        private EmployeeLeaveType GetEmployeeLeaveType(int leaveTypeId, string reqiestEmployeeId)
            => this.db.EmployeesLeaveTypes
              .Include(x => x.LeaveType)
              .Where(el => el.EmployeeId == reqiestEmployeeId &&
                           el.LeaveTypeId == leaveTypeId)
              .FirstOrDefault();

        private IEnumerable<LeaveServiceModel> GetLeaves(IQueryable<Leave> query)
            => query
                .ProjectTo<LeaveServiceModel>(this.mapper)
                .ToList();
    }
}
