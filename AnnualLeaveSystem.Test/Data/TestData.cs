namespace AnnualLeaveSystem.Test.Data
{
    using System;
    using AnnualLeaveSystem.Data.Models;
    using AnnualLeaveSystem.Test.Mocks;

    public static class TestData
    {
        public static string UserId => "1b99c696-64f5-443a-ae1e-6b4a1bc8f2cb";

        public static Employee GetUser()
        {
            return new Employee
            {
                Id = "1b99c696-64f5-443a-ae1e-6b4a1bc8f2cb",
                UserName = "testuser",
                Email = "testuser@abv.bg",
                PasswordHash = "sdasd324olkk34dff",
            };
        }

        public static void ImportHolidays()
        {
            var data = DatabaseMock.Instance;

            var holidays = new[]
            {
                new OfficialHoliday
                {
                    Date = new DateTime(2021, 01, 01).Date,
                    Name = "New Year`s Day",
                },
                new OfficialHoliday
                {
                    Date = new DateTime(2021, 03, 03).Date,
                    Name = "Liberation Day",
                },
                new OfficialHoliday
                {
                    Date = new DateTime(2021, 04, 30).Date,
                    Name = "Good Friday",
                },
                new OfficialHoliday
                {
                    Date = new DateTime(2021, 05, 01).Date,
                    Name = "International Workers Day",
                },
                new OfficialHoliday
                {
                    Date = new DateTime(2021, 05, 02).Date,
                    Name = "Easter",
                },
                new OfficialHoliday
                {
                    Date = new DateTime(2021, 05, 03).Date,
                    Name = "Easter",
                },
                new OfficialHoliday
                {
                    Date = new DateTime(2021, 05, 04).Date,
                    Name = "Easter",
                },
                new OfficialHoliday
                {
                    Date = new DateTime(2021, 05, 06).Date,
                    Name = "St George`s Day",
                },
                new OfficialHoliday
                {
                    Date = new DateTime(2021, 05, 24).Date,
                    Name = "Sts Cyril and Methodius Day",
                },
                new OfficialHoliday
                {
                    Date = new DateTime(2021, 09, 06).Date,
                    Name = "Unification Day",
                },
                new OfficialHoliday
                {
                    Date = new DateTime(2021, 09, 22).Date,
                    Name = "Independence Day",
                },
                new OfficialHoliday
                {
                    Date = new DateTime(2021, 12, 24).Date,
                    Name = "Christmas",
                },
                new OfficialHoliday
                {
                    Date = new DateTime(2021, 12, 25).Date,
                    Name = "Christmas",
                },
                new OfficialHoliday
                {
                    Date = new DateTime(2021, 12, 26).Date,
                    Name = "Christmas",
                },
                new OfficialHoliday
                {
                    Date = new DateTime(2021, 12, 27).Date,
                    Name = "Christmas",
                },
                new OfficialHoliday
                {
                    Date = new DateTime(2021, 12, 28).Date,
                    Name = "Christmas",
                }
            };

            data.OfficialHolidays.AddRange(holidays);

            data.SaveChanges();
        }
    }
}