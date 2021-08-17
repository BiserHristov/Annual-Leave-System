namespace AnnualLeaveSystem.Services.Holidays
{
    using System.ComponentModel.DataAnnotations;

    using static AnnualLeaveSystem.Data.DataConstants.Holiday;

    public class HolidayServiceModel
    {
        public int Id { get; init; }

        [Required]
        [DataType(DataType.Date)]
        public string Date { get; init; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]

        public string Name { get; init; }

    }
}