namespace AnnualLeaveSystem.Areas.Admin
{
    public class AdminConstants
    {
        public const string AreaName = "Admin";
        public const string AdministratorRoleName = "Administrator";

        public class Holidays
        {
            public const string NotNextYearMessage = "The date is not for the next year.";
            public const string AlreadyExistMessage = "The date already exist.";
            public const string NotValidDateMessage = "This date is not valid.";
        }

        public class Users
        {
            public const string InvalidTeamIdMessage = "Team Id is not valid.";
            public const string InvalidDepartmentIdMessage = "Department Id is not valid.";
            public const string InvalidDateMessage = "Date is not valid.";
            public const string InvalidPasswordsMatchMessage = "Password and confirm password does not match!";
            public const string ExistingEmailMessage = "Email is already taken.";
            public const string InvalidCredentialsMessage = "Credentials are not valid";
        }
    }
}