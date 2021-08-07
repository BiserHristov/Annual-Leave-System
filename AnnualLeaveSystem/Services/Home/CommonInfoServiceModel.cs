namespace AnnualLeaveSystem.Services.Statistics
{
    public class CommonInfoServiceModel
    {
        public int EmployeesCount { get; init; }
        public int ApprovedLeaveCount { get; init; }
        public int InProgressLeaveCount { get; init; }
        public int AllLeavesTotalDays { get; init; }

        public int MissingEmployees { get; init; }
    }
}
