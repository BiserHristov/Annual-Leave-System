namespace AnnualLeaveSystem.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class OfficialHoliday
    {
        public int Id { get; init; }

        [Column(TypeName = "date")]
        public DateTime Date { get; init; }

        public string Name { get; init; }

    }
}
