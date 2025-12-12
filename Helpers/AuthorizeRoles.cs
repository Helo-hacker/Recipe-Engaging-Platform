using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RecipeSharingApp.Helpers
{
    public class AuthorizeRoles : Attribute, IAuthorizationFilter
    {
        private readonly string[] _roles;

        public AuthorizeRoles(params string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var role = context.HttpContext.Session.GetString("UserRole");
            if (role == null || !_roles.Contains(role))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
        }
    }
}
