using Application.Important_Area;
using Application.Services.Interfaces;
using Domain.Models;
using Infra.RequestApi.Interface;

namespace Application.Services
{
    public class ConfigureOptionsService : InterConfigureOptionsServices
    {

        readonly string routeMgmt = String.Concat(ApiRouteMgmt.Link, ApiRouteMgmt.Port, ApiRouteMgmt.Api);
        
        private readonly InterGeneralApi _requestApi;

        public ConfigureOptionsService(InterGeneralApi requestApi)
        {
            _requestApi = requestApi;
        }

        public async Task<List<vmDrivers>> GetDriversOptions()
        {
            return await _requestApi.GetAsync<List<vmDrivers>>($"{routeMgmt}/v1/ConfigureOptions/GetDriversOptions") ?? [];
        }

        public async Task<List<vmCpuTypes>> GetCpuTypesOptions()
        {
            return await _requestApi.GetAsync<List<vmCpuTypes>>($"{routeMgmt}/v1/ConfigureOptions/GetCpuTypesOptions") ?? [];
        }
    }
}
