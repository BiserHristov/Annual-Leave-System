namespace AnnualLeaveSystem.Data.Models
{
    using System.Collections.Generic;
    public class Team
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public Project Project { get; set; }

        public ICollection<Employee> Employees { get; init; } = new HashSet<Employee>();
    }
}
