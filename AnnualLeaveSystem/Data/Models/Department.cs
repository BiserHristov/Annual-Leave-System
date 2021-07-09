namespace AnnualLeaveSystem.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static AnnualLeaveSystem.Data.DataConstants;
    public class Department
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(DepartmentNameMaxLength)]
        public string Name { get; set; }

        public ICollection<Employee> Employees { get; init; } = new HashSet<Employee>();
    }
}
