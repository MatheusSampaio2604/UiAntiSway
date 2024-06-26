﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;

namespace UI.Models.ActionsFilter
{
    public class AuthorizeTokenAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var token = context.HttpContext.Request.Cookies["name"];
            if (string.IsNullOrEmpty(token))
            {
                context.Result = new RedirectToActionResult("Login", "User", null);
                return;
            }

            try
            {
                //var handler = new JwtSecurityTokenHandler();
                //var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
                //if (jwtToken == null)
                //{
                //    context.Result = new RedirectToActionResult("Login", "User", null);
                //    return;
                //}

                //// Optionally, validate token expiration, issuer, audience, etc.
                //if (jwtToken.ValidTo < DateTime.UtcNow)
                //{
                //    context.Result = new RedirectToActionResult("Login", "User", null);
                //    return;
                //}
            }
            catch
            {
                context.Result = new RedirectToActionResult("Login", "User", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}