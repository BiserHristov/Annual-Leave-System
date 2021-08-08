namespace AnnualLeaveSystem.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static AnnualLeaveSystem.Areas.Admin.AdminConstants;

    [Area(AdminConstants.AreaName)]
    [Authorize(Roles = AdministratorRoleName)]
    public class BaseAdminController : Controller
    {

    }
}
