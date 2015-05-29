using Microsoft.AspNet.Identity;
using System.Security.Authentication;
using System.Security.Principal;

namespace Simplr.Core.Web.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetUserId(this IPrincipal user)
        {
            if (user?.Identity == null)
            {
                throw new AuthenticationException();
            }
            return user.Identity.GetUserId();
        }
    }
}
