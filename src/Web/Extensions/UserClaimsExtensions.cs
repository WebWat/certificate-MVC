using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Web.Extensions;

public static class UserClaimsExtensions
{
    public static bool IsRole(this IEnumerable<Claim> claims, string role)
        => claims.First(c => c.Type == ClaimsIdentity.DefaultRoleClaimType).Value.Contains(role);
}
