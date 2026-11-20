using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RaceDay.Mvc.Services;

public class SessionAuthorizeAttribute : ActionFilterAttribute
{
    private readonly string[] _allowedRoles;

    public SessionAuthorizeAttribute()
    {
        _allowedRoles = Array.Empty<string>();
    }

    public SessionAuthorizeAttribute(params string[] roles)
    {
        _allowedRoles = roles;
    }

    public override void OnActionExecuting(
        ActionExecutingContext context)
    {
        var httpContext = context.HttpContext;

        var token = httpContext.Session.GetString(
            SessionKeys.JwtToken);

        var role = httpContext.Session.GetString(
            SessionKeys.Role);

        // Check whether the user is logged in
        if (string.IsNullOrWhiteSpace(token))
        {
            context.Result = new RedirectToActionResult(
                "Login",
                "Auth",
                new
                {
                    returnUrl = httpContext.Request.Path
                });

            return;
        }

        // Check role if roles have been specified
        if (_allowedRoles.Length > 0 &&
            !_allowedRoles.Any(r =>
                string.Equals(
                    r,
                    role,
                    StringComparison.OrdinalIgnoreCase)))
        {
            if (context.Controller is Controller controller)
            {
                controller.TempData["Error"] =
                    "You do not have permission to access that page.";
            }

            context.Result = new RedirectToActionResult(
                "Index",
                "Home",
                null);

            return;
        }

        base.OnActionExecuting(context);
    }
}