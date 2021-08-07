namespace AnnualLeaveSystem.Areas.Admin.Services.Employees
{
    using AnnualLeaveSystem.Services.Users;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class EditEmployeeServiceModel : BaseEmployeeServiceModel
    {
        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public IEnumerable<RegisterDepartamentServiceModel> Departments { get; set; } = new HashSet<RegisterDepartamentServiceModel>();

        public IEnumerable<int> Teams { get; set; } = new HashSet<int>();
    }
}
