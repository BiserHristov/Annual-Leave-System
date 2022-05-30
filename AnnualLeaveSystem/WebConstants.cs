namespace AnnualLeaveSystem
{
    public class WebConstants
    {
        public const string UserRoleName = "User";
        public const string TeamLeadRoleName = "TeamLead";
        public const string DateFormat = "dd.MM.yyyy";

        public class Email
        {
            public const string RequestSubject = "Leave request action required";
            public const string StatusChanged = "Leave request status changed";
            public const string ContentText = "A leave request is waiting for your approval: ";
        }

        public class Cache
        {
            public const string AllHolidayDatesCacheKey = nameof(AllHolidayDatesCacheKey);
            public const string AllHolidaysCacheKey = nameof(AllHolidaysCacheKey);
        }

        public class Leaves
        {
            public const string OfficialHolidayMessage = "This date is official holiday ({name})";
            public const string StartBeforeEndDateMessage = @"""Start date"" should be before ""End date"".";
            public const string EndAfterStartDateMessage = @"""End date"" should be after ""Start date"".";
            public const string AfterOrEqualTodayMessage = " should be after or equal to todays' date.";
            public const string IncorrectTotalDaysMessage = "Count of days is not correct or it is equal to zero.";
            public const string IncorrectLeaveTypeMessage = "Leave type does not exist.";
            public const string NotExistingEmployeeInTeamMessage = "There is no such employee in your team.";
            public const string InsufficientLeaveDaysLeftMessage = "You do no have enough days left from the selected leave type option.";
            public const string ExistingRequestInsidePeriodMessage = "You have already Leave Request inside the given period.";
            public const string ExistingRequestForDateMessage = "You already have Leave Request for this date.";
            public const string AlreadySubstituteForDateMessage = "You are already substitute for this date.";
            public const string ExistingSubstituteRequestInsidePeriodMessage = "You are already substitute inside the given period.";
            public const string NotAuthorizeMessage = "You are not authorized to perform this action.";
        }
    }
}
