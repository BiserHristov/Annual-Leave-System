namespace AnnualLeaveSystem.Models.Leaves
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class LeaveDetailsViewModel
    {

        public DateTime RequestDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int TotalDays { get; set; }

        public string Type { get; set; }
        public string RequestEmployeeName { get; set; }

        public string SubstituteEmployeeName { get; set; }

        public string ApproveEmployeeName { get; set; }

        public string Status { get; set; }

        public string Comments { get; set; }
    }
}
