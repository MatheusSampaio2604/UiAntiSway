using Application.Services.Interface;
using Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace UI.Controllers
{
    [AllowAnonymous]
    public class UserController : Controller
    {
        private readonly InterLoginService _interLoginService;
        private readonly InterRegisterService _interRegisterService;

        public UserController(InterLoginService interLoginService, InterRegisterService interRegisterService)
        {
            _interLoginService = interLoginService;
            _interRegisterService = interRegisterService;
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
                    var cookieOptions = CreateCookieOptions();

                    CreateCookieUser(loginResult, cookieOptions);

                    return RedirectToAction("Index", "Home");
                }
                else
                    return Unauthorized(new { success = false, message = string.Join(", ", loginResult.Errors) });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred. Please try again later.", error = ex.Message });
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

        private void CreateCookieUser(LoginResult loginResult, CookieOptions cookieOptions)
        {
            Response.Cookies.Append("token", loginResult.Token, cookieOptions);
            Response.Cookies.Append("name", loginResult.Name, cookieOptions);

            var roles = GetRolesFromToken(loginResult.Token);
            Response.Cookies.Append("roles", string.Join(",", roles), cookieOptions);
        }

        private IEnumerable<string> GetRolesFromToken(string token)
        {
            JwtSecurityTokenHandler handler = new();
            var jwtToken = handler.ReadJwtToken(token);
            var roles = jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            return roles;
        }

        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("token");
            Response.Cookies.Delete("name");
            Response.Cookies.Delete("roles");

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(vmRegisterUser registerUser)
        {
            try
            {
                bool registerResult = await _interRegisterService.RegisterAsync(registerUser);

                if (registerResult)
                {
                    vmLoginUser loginUser = new() { UserName = registerUser.Email, Password = registerUser.Password };

                    LoginResult loginResult = await _interLoginService.LoginAsync(loginUser);

                    if (loginResult.Succeeded)
                    {
                        var cookieOptions = CreateCookieOptions();

                        CreateCookieUser(loginResult, cookieOptions);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return Unauthorized(new { success = false, message = string.Join(", ", loginResult.Errors) });
                    }
                }
                else
                    return BadRequest(new { success = false, message = string.Join(", ", registerResult) });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred. Please try again later.", error = ex.Message });
            }
        }
    }
}
