namespace AnnualLeaveSystem.Services.Leaves
{
    using System.Collections.Generic;

    public class EditLeaveServiceModel : BaseServiceModel
    {

        public int Id { get; init; }

        public int LeaveTypeId { get; init; }

        public string ApproveEmployeeId { get; set; }

        public string Comments { get; init; }

        public IEnumerable<LeaveTypeServiceModel> LeaveTypes { get; set; }

        public IEnumerable<SubstituteEmployeeServiceModel> EmployeesInTeam { get; set; }
    }
}
