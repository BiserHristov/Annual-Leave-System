namespace AnnualLeaveSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;

    public class OfficialHoliday
    {
        public int Id { get; init; }

        [Column(TypeName = "date")]
        public DateTime Date { get; init; }

        public string Name { get; init; }

    }
}
