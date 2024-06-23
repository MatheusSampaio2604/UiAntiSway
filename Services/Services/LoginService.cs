using Application.Services.Interface;
using Application.ViewModels;
using Infra.RequestApi.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Important_Area;

namespace Application.Services
{
    internal class LoginService : InterLoginService
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
