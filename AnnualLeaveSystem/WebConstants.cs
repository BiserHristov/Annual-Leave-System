namespace AnnualLeaveSystem
{
    public class WebConstants
    {
        public const string UserRoleName = "User";
        public const string TeamLeadRoleName = "TeamLead";

        public const string EmailRequestSubject = "Leave request action required";
        public const string EmailStatusChanged = "Leave request status changed";

        public const string DateFormat = "dd.MM.yyyy";
        public class Cache
        {
            public const string AllHolidayDatesCacheKey = nameof(AllHolidayDatesCacheKey);
            public const string AllHolidaysCacheKey = nameof(AllHolidaysCacheKey);
        }

        public class Leaves
        {
            public const string OfficialHolidayMessage ="This date is official holiday ({name})";
        }
    }
}
