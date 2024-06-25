using Application.Important_Area;
using Application.Services.Interface;
using Application.ViewModels;
using Infra.RequestApi.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Application.Services
{
    public class ConfigurationService : InterConfigurationService
    {
        private readonly InterGeneralApi _GeneralApi;
        readonly string routeMgmt = String.Concat(ApiRouteMgmt.Link, ApiRouteMgmt.Port, ApiRouteMgmt.Api);

        public ConfigurationService(InterGeneralApi generalApi)
        {
            _GeneralApi = generalApi;
        }

        public async Task<vmWriteReadPlc> ReadPlcAsync(string address)
        {
            var dados = await _GeneralApi.GetAsync<vmWriteReadPlc>($"{routeMgmt}/v1/Plc/getValueFromPlc");
            
            return dados;
        }

        public async Task<bool> WritePlcAsync(vmWriteReadPlc plcData)
        {
            var dados = await _GeneralApi.PostAsync<vmWriteReadPlc, bool>($"/v1", plcData);

            return dados;
        }


        public async Task<List<vmPlc>> GetListPlc()
        {
            var dados = await _GeneralApi.GetAsync<List<vmPlc>>($"{routeMgmt}/v1/Plc/getListPlc");

            return dados;
        }

        public async  Task<vmPlcSettings> GetSettingsPlc()
        {
            vmPlcSettings? dados = await _GeneralApi.GetAsync<vmPlcSettings>($"{routeMgmt}/v1/Plc/GetSettingsPlc");

            return dados;
        }

        public async Task<vmPlcSettings> UpdateSettingsPlc(vmPlcSettings settings)
        {
            var dados = await _GeneralApi.PostAsync<vmPlcSettings, vmPlcSettings>($"{routeMgmt}/v1/Plc/UpdateSettingsPlc", settings);

            return dados;
        }

        public async Task<bool> TestConnectionPlc()
        {
            bool dados = await _GeneralApi.GetAsync<bool>($"{routeMgmt}/v1/Plc/TestConnectionPlc");

            return dados;
        }
    }
}
