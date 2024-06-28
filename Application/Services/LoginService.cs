using Application.Important_Area;
using Application.Services.Interface;
using Application.ViewModels;
using Infra.RequestApi.Interface;

namespace Application.Services
{
    public class LoginService : InterLoginService
    {
        private readonly InterGeneralApi _InterfaceApi;

        readonly string routeMgmt = String.Concat(ApiRouteMgmt.Link, ApiRouteMgmt.Port, ApiRouteMgmt.Api);

        public LoginService(InterGeneralApi InterfaceApi)
        {
            _InterfaceApi = InterfaceApi;
        }

        public async Task<LoginResult> LoginAsync(vmLoginUser loginUser)
        {
            var result = await _InterfaceApi.PostAsync<vmLoginUser, LoginResult>($"{routeMgmt}/v1/User/login", loginUser);
            return result;
        }
    }
}
