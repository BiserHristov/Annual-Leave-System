using AnnualLeaveSystem.Areas.Admin;

namespace AnnualLeaveSystem.Infrastructure
{
    using System.Security.Claims;
    using static WebConstants;
    using static AdminConstants;
    public static class ClaimsPrincipalExtensions
    {
        public static string GetId(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.NameIdentifier).Value;

        public static bool IsAdmin(this ClaimsPrincipal user)
            => user.IsInRole(AdministratorRoleName);
        public static bool IsTeamLead(this ClaimsPrincipal user)
            => user.IsInRole(TeamLeadRoleName);

        public static bool IsUser(this ClaimsPrincipal user)
            => user.IsInRole(UserRoleName);
    }
}
