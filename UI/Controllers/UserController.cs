using Application.Services.Interface;
using Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace UI.Controllers
{
    [AllowAnonymous]
    public class UserController : Controller
    {
        private readonly InterLoginService _interLoginService;

        public UserController(InterLoginService interLoginService)
        {
            _interLoginService = interLoginService;
        }

        private void TempDataErrorMessage(string message)
        {
            TempData["ErrorMessage"] = message.Trim();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new vmLoginUser());
        }

        [HttpPost]
        public async Task<IActionResult> Login(vmLoginUser loginUser)
        {
            try
            {
                //var loginResult = await _interLoginService.LoginAsync(loginUser);

                //if (loginResult.Succeeded)
                //{
                //    var cookieOptions = CreateCookieOptions();

                //    CreateCookieUser(loginResult, cookieOptions);

                //    return RedirectToAction("Index", "Home");
                //}
                //else
                //    return Unauthorized(new { success = false, message = string.Join(", ", loginResult.Errors) });

                if (loginUser != null && !String.IsNullOrEmpty(loginUser.UserName)
                    && !String.IsNullOrEmpty(loginUser.Password) && loginUser.Password == "Janus90")
                {
                    var cookieOptions = CreateCookieOptions();

                    Response.Cookies.Append("name", loginUser.UserName, cookieOptions);

                    return RedirectToAction("Monitoring", "Management");
                }
                else throw new Exception("User or Password is incorrect!");
            }
            catch (Exception ex)
            {
                TempDataErrorMessage(ex.Message);
                return RedirectToAction("Login", "User");
                //return StatusCode(500, new { success = false, message = "An error occurred. Please try again later.", error = ex.Message });
            }
        }

        private CookieOptions CreateCookieOptions()
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTimeOffset.UtcNow.AddHours(8)
            };
        }

        //private void CreateCookieUser(LoginResult loginResult, CookieOptions cookieOptions)
        //{
        //    Response.Cookies.Append("token", loginResult.Token, cookieOptions);
        //    Response.Cookies.Append("name", loginResult.Name, cookieOptions);

        //    var roles = GetRolesFromToken(loginResult.Token);
        //    Response.Cookies.Append("roles", string.Join(",", roles), cookieOptions);
        //}

        //private IEnumerable<string> GetRolesFromToken(string token)
        //{
        //    JwtSecurityTokenHandler handler = new();
        //    var jwtToken = handler.ReadJwtToken(token);
        //    var roles = jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
        //    return roles;
        //}

        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("token");
            Response.Cookies.Delete("name");
            Response.Cookies.Delete("roles");

            return RedirectToAction("Login", "User");
        }

    }
}
