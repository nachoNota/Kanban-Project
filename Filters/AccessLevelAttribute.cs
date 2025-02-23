using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AccessLevelAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _requiredAccessLevels;

        public AccessLevelAttribute(params RolUsuario[] requiredAccessLevels)
        {
            _requiredAccessLevels = requiredAccessLevels.Select(acc => acc.ToString()).ToArray();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if(!IsAuthenticated(context))
            {
                context.Result = new RedirectToActionResult("Index", "Login", null);
                return;
            }
         
            var userAccessLevel = GetUserAccessLevel(context);
            bool esAccessLevel = _requiredAccessLevels.Contains(userAccessLevel);
            bool SinNivelesDefinidos = _requiredAccessLevels is null || _requiredAccessLevels.Length == 0;

            if (!esAccessLevel || SinNivelesDefinidos)
            {
                context.Result = new RedirectToActionResult("ErrorRol", "Error", null);
                return;
            }
        }

        private static string? GetUserAccessLevel(AuthorizationFilterContext context)
            => context.HttpContext.Session.GetString("AccessLevel") ?? string.Empty;

        private static bool IsAuthenticated(AuthorizationFilterContext context) => context.HttpContext.Session.GetString("IsAuthenticated") == "true";
    }
}
