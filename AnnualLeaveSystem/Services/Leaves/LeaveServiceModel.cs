using System;

namespace AnnualLeaveSystem.Services.Leaves
{
    public class LeaveServiceModel : BaseServiceModel
    {
    
        public string FirstName { get; init; }

        public string LastName { get; init; }

        public string Status { get; init; }

        public string ApproveEmployeeId { get; init; }

        public DateTime RequestDate { get; init; }
    }


}
