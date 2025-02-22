﻿namespace AnnualLeaveSystem.Infrastructure
{
    using System.Security.Claims;
    using static AnnualLeaveSystem.Areas.Admin.AdminConstants;
    using static AnnualLeaveSystem.WebConstants;

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
