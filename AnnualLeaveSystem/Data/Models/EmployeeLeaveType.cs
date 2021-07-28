namespace AnnualLeaveSystem.Data.Models
{
    public class EmployeeLeaveType
    {
        public EmployeeLeaveType()
        {

        }
        public string EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int LeaveTypeId { get; set; }
        public LeaveType LeaveType { get; set; }

        public int UsedDays { get; set; }

        public int RemainingDays
        {
            get
            {
                return this.LeaveType.DefaultDays - this.UsedDays;
            }
        }

        //public ICollection<Employee> Employees { get; set; }
        //public ICollection<LeaveType> LeaveTypes { get; set; }

    }
}
