using System.Security.Claims;
using System.Security.Principal;

namespace CleanCodeScaffold.Api.Util
{
    public static class IdentityExtensions
    {
        public static long GetId(this ClaimsPrincipal identity)
        {
           var claim = identity.Claims.FirstOrDefault(x => x.Type.ToLower().Equals("id"));
            string id = claim == null ? "0" : claim.Value;
            return Convert.ToInt64(id);
        }
    }
}
