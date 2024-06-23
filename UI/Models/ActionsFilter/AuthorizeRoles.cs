using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace UI.Models.ActionsFilter
{
    public class AuthorizeRolesAttribute : TypeFilterAttribute
    {
        public AuthorizeRolesAttribute(params string[] roles) : base(typeof(AuthorizeRolesFilter))
        {
            Arguments = new object[] { roles };
        }
    }

    public class AuthorizeRolesFilter : IAuthorizationFilter
    {
        readonly string[] _roles;

        public AuthorizeRolesFilter(params string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            var rolesFromCookie = GetRolesFromCookie(context.HttpContext);
            
            foreach (var role in _roles)
            {
                if (rolesFromCookie.Contains(role))
                {
                    return; // Autorização bem-sucedida
                }
            }

            // Se não pertence a nenhum dos roles especificados, nega o acesso
            context.Result = new ForbidResult();
        }

        private string[] GetRolesFromCookie(HttpContext context)
        {
            var rolesCookie = context.Request.Cookies["roles"];
            if (!string.IsNullOrEmpty(rolesCookie))
            {
                return rolesCookie.Split(','); // Separador utilizado para o string.join no backend
            }
            return new string[0];
        }
    }
}
