namespace AnnualLeaveSystem.Areas.Admin.Services.Teams
{
    using System.Collections.Generic;

    public interface ITeamService
    {
        public IEnumerable<int> All();
    }
}
