namespace AnnualLeaveSystem.Services.EmployeeLeaveTypes
{
    public class EmployeeLeaveTypesServiceModel
    {
        public int UsedDays { get; set; }

        public int PendingApprovalDays { get; set; }

        public int RemainingDays { get; init; }

    }
}