namespace AnnualLeaveSystem
{
    public class WebConstants
    {
        public const string UserRoleName = "User";
        public const string TeamLeadRoleName = "TeamLead";

        public const string EmailRequestSubject = "Leave request action required";
        public const string EmailStatusChanged = "Leave request status changed";

        public class Cache
        {
            public const string AllHolidayDatesCacheKey = nameof(AllHolidayDatesCacheKey);
            public const string AllHolidaysCacheKey = nameof(AllHolidaysCacheKey);
        }
    }
}
