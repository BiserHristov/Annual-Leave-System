namespace AnnualLeaveSystem.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static AnnualLeaveSystem.Data.DataConstants.Holiday;
    public class OfficialHoliday
    {
        public int Id { get; init; }

        [Required]
        [Column(TypeName = "Date")]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

    }
}
