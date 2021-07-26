namespace AnnualLeaveSystem.Services.Statistics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CommonInfoServiceModel
    {
        public int EmployeesCount { get; init; }
        public int ApprovedLeaveCount { get; init; }
        public int InProgressLeaveCount { get; init; }
        public int AllLeavesTotalDays { get; init; }
    }
}
