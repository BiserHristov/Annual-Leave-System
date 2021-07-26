namespace AnnualLeaveSystem.Data
{
    public class DataConstants
    {
        public class Department
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 20;
        }
        public class Employee
        {
            public const int PasswordMinLength = 6;
            public const int PasswordMaxLength = 30;
            public const int FirstNameMinLength = 2;
            public const int FirstNameMaxLength = 15;
            public const int MiddleNameMinLength = 0;
            public const int MiddleNameMaxLength = 15;
            public const int LastNameMinLength = 2;
            public const int LastNameMaxLength = 20;
            public const int JobTitleMinLength = 3;
            public const int JobTitleMaxLength = 25;
        }

        public class LeaveType
        {
            public const int NameMinLength = 5;
            public const int NameMaxLength = 25;
            public const int DefaultDaysMinValue = 2;
            public const int DefaultDaysMaxValue = 25;
        }

        public class Project
        {
            public const int NameMinLength = 5;
            public const int NameMaxLength = 20;
        }


        public class User
        {
            

            public const int _EmployeeTeamId = 2;
        }

    }
}
