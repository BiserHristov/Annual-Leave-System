namespace AnnualLeaveSystem.Services.LeaveTypes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ILeaveTypeService
    {
        public bool TypeExist(int id);
    }
}
