namespace AnnualLeaveSystem.Services.Leaves
{
    using AnnualLeaveSystem.Data;
    using AnnualLeaveSystem.Data.Models;
    using AnnualLeaveSystem.Models.Leaves;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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


            var leaves = leavesQuery
                .Skip((currentPage - 1) * leavesPerPage)
                .Take(leavesPerPage)
                .Select(l => new LeaveServiceModel
                {
                    Id = l.Id,
                    FirstName = l.RequestEmployee.FirstName,
                    LastName = l.RequestEmployee.LastName,
                    StartDate = l.StartDate.ToLocalTime().ToShortDateString(),
                    EndDate = l.EndDate.ToLocalTime().ToShortDateString(),
                    TotalDays = l.TotalDays,
                    Status = l.LeaveStatus.ToString(),
                    RequestDate = l.RequestDate.ToLocalTime().ToShortDateString(),
                })
                .ToList();

            return new LeaveQueryServiceModel
            {
                FirstName = firstName,
                LastName = lastName,
                Sorting = sorting,
                Status = status,
                CurrentPage = currentPage,
                LeavesPerPage = leavesPerPage,
                Leaves = leaves
            };
        }
    }
}
