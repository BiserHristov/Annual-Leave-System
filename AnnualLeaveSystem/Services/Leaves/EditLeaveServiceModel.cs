namespace AnnualLeaveSystem.Services.Leaves
{
    using AnnualLeaveSystem.Services.Leaves;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class EditLeaveServiceModel : BaseServiceModel
    {

        public int LeaveTypeId { get; init; }

        public string ApproveEmployeeId { get; set; }

        public string Comments { get; init; }

        public IEnumerable<LeaveTypeServiceModel> LeaveTypes { get; set; }


        public IEnumerable<SubstituteEmployeeServiceModel> EmployeesInTeam { get; set; }
    }
}
