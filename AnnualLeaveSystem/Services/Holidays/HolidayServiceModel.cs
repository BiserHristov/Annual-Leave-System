namespace AnnualLeaveSystem.Services.Holidays
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using static AnnualLeaveSystem.Data.DataConstants.Holiday;

    public class HolidayServiceModel
    {
        public int Id { get; init; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; init; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; init; }

    }
}