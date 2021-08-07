namespace AnnualLeaveSystem.Areas.Admin.Services.Teams
{
    using System.Collections.Generic;
    using System.Linq;
    using AnnualLeaveSystem.Data;

    public class TeamService : ITeamService
    {
        private readonly LeaveSystemDbContext db;

        public TeamService(LeaveSystemDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<int> All()
        {
            return this.db.Teams
                .Select(t => t.Id)
                .ToList();
        }
    }
}
