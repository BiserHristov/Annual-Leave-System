namespace AnnualLeaveSystem.Areas.Admin.Services.Teams
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ITeamService
    {
        public IEnumerable<int> All();
    }
}
