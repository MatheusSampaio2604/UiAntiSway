using Application.Services.Interface;
using Application.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
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
        public async Task<LoginResult> Login(vmLoginUser loginUser)
        {
            try
            {
                var user = await _interLoginService.LoginAsync(loginUser);

                return user;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
