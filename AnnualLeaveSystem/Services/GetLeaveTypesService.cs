namespace AnnualLeaveSystem.Services
{
    using AnnualLeaveSystem.Data;
    using AnnualLeaveSystem.Models.Leaves;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class GetLeaveTypesService : IGetLeaveTypesService
    {
        private readonly LeaveSystemDbContext db;

        public GetLeaveTypesService(LeaveSystemDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<LeaveTypeViewModel> GetLeaveTypes()
        {
            return this.db.LeaveTypes
                 .Select(l => new LeaveTypeViewModel
                 {
                     Id = l.Id,
                     Name = l.Name
                 })
                 .ToList();
        }
    }
}
