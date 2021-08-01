namespace AnnualLeaveSystem.Services.Emails
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class EmailServiceModel
    {
        public string StartDate { get; init; }
        public string EndDate { get; init; }
        public string RequestEmployeeName { get; init; }
        public int TotalDays { get; init; }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Request employee: {RequestEmployeeName}");
            sb.AppendLine($"From: {StartDate}");
            sb.AppendLine($"To: {EndDate}");
            sb.AppendLine($"Total business days: {TotalDays}");

            return sb.ToString().Trim();
        }
    }
}
