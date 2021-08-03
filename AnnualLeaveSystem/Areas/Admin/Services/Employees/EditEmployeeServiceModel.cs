namespace AnnualLeaveSystem.Areas.Admin.Services.Employees
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class EditEmployeeServiceModel : BaseEmployeeServiceModel
    {
        [Required]
        public int DepartmentId { get; set; }
        public IEnumerable<int> DepartmentIDs { get; set; } = new HashSet<int>();

        public IEnumerable<int> TeamIDs { get; set; } = new HashSet<int>();
    }
}
