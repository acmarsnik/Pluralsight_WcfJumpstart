using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Zza.Constants
{
    public static class Sids
    {
        public const string Everyone = "S-1-1-0";
        public const string AuthenticatedUser = "S-1-5-11";
        public const string Admin = "S-1-5-32-544";
        public const string GroupType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/groupsid";


        public static List<string> GetRoleValues(IPrincipal principal)
        {
            ClaimsIdentity userIdentity = (ClaimsIdentity)principal.Identity;
            IEnumerable<Claim> claims = userIdentity.Claims;
            string roleClaimType = userIdentity.RoleClaimType;
            //var roles = claims.Where(c => c.Type == ClaimTypes.Role).ToList();
            List<Claim> roles = claims.Where(c => c.Type == roleClaimType).ToList();
            List<string> roleValues = roles.Select(r => r.Value).ToList();

            return roleValues;
        }

        public static void DebugLogAllSidRoleValuesForUser(IPrincipal principal)
        {
            List<string> roles = GetRoleValues(principal);

            foreach (string role in roles)
            {
                /* Go to: https://support.microsoft.com/en-us/help/243330/well-known-security-identifiers-in-windows-operating-systems 
                 * to see what the SID value for the role means 
                 */
                Debug.WriteLine("Role SID value: " + role);
            }
        }

        public static bool HasAdminClaim()
        {
            bool hasAdminClaim = ClaimsPrincipal.Current.HasClaim(GroupType, Admin);
            return hasAdminClaim;
        }
    }
}
