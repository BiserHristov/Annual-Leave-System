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

        public DateTime Date { get; set; }

        public string Name { get; set; }

    }
}
