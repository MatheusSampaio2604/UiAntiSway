using Application.Important_Area;
using Application.Services.Interface;
using Application.ViewModels;
using Infra.RequestApi.Interface;

namespace Application.Services
{
    public class RegisterService : InterRegisterService
    {
        private readonly InterGeneralApi _InterfaceApi;

        readonly string routeMgmt = String.Concat(ApiRouteMgmt.Link, ApiRouteMgmt.Port, ApiRouteMgmt.Api);

        public RegisterService(InterGeneralApi InterfaceApi)
        {
            _InterfaceApi = InterfaceApi;
        }

        public async Task<bool> RegisterAsync(vmRegisterUser registerUser)
        {
            var result = await _InterfaceApi.PostAsync<vmRegisterUser, LoginResult>($"{routeMgmt}/v1/User/register", registerUser);
            if (result.Succeeded) return true;
            else return false;

        }
    }
}
