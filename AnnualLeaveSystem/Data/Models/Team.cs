namespace AnnualLeaveSystem.Data.Models
{
    using System.Collections.Generic;
    public class Team
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public int? ProjectId { get; init; }

        public Project Project { get; init; }

        public ICollection<Employee> Employees { get; init; } = new HashSet<Employee>();
    }
}
