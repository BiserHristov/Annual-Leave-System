namespace AnnualLeaveSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static AnnualLeaveSystem.Data.DataConstants;


    public class Project
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(ProjectNameMaxLength)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        //public int TeamId { get; set; }

        //public Team Team { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        public ICollection<Team> Teams { get; init; } = new HashSet<Team>();
    }
}
