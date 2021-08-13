namespace AnnualLeaveSystem.Test.Data
{
    using AnnualLeaveSystem.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class Leaves
    {
        public static string substituteEmployeeId = Guid.NewGuid().ToString();
        public static IEnumerable<Leave> TenApprovedLeaves()
        {
            return Enumerable.Range(0, 10).Select(x => new Leave()
            {
                LeaveStatus = Status.Approved
            });
        }
    }
}
