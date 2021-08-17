namespace AnnualLeaveSystem.Test.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using AnnualLeaveSystem.Data.Models;

    public static class LeaveTestData
    {
        //public static string substituteEmployeeId = Guid.NewGuid().ToString();
        public static IEnumerable<Leave> TenApprovedLeaves()
        {
            return Enumerable.Range(0, 10).Select(x => new Leave()
            {
                LeaveStatus = Status.Approved
            });
        }
    }
}
