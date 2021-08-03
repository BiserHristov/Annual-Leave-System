namespace AnnualLeaveSystem.Areas.Admin.Services.Employees
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class EmployeeServiceModel : BaseEmployeeServiceModel
    {

        public string DepartmentName { get; set; }

        public string TeamLeadName { get; set; }

    }
}
