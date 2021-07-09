namespace AnnualLeaveSystem.Services
{
    using AnnualLeaveSystem.Models.Leaves;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IGetLeaveTypesService
    {
        public IEnumerable<LeaveTypeViewModel> GetLeaveTypes();
    }
}
