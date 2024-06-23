using Application.Services.Interface;
using Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(vmLoginUser loginUser)
        {
            try
            {
                var loginResult = await _interLoginService.LoginAsync(loginUser);

                if (loginResult.Succeeded)
                {
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTimeOffset.UtcNow.AddHours(8)
                    };
                    Response.Cookies.Append("token", loginResult.Token, cookieOptions);
                    Response.Cookies.Append("name", loginResult.Name, cookieOptions);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Unauthorized(new { success = false, message = string.Join(", ", loginResult.Errors) });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred. Please try again later.", error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
    }
}
