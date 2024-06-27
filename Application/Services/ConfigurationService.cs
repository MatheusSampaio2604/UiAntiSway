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

        public async Task<vmWritePlc> ReadPlcAsync(string address)
        {
            var dados = await _GeneralApi.GetAsync<vmWritePlc>($"{routeMgmt}/v1/Plc/getValueFromPlc/{address}");

            return dados;
        }

        public async Task<bool> WritePlcAsync(List<vmApiRequestWritePlc> plcData)
        {
            var dados = await _GeneralApi.PostAsync<List<vmApiRequestWritePlc>, bool>($"{routeMgmt}/v1/Plc/SetValueToPlc", plcData);

            return dados;
        }


        public async Task<List<vmPlc>> GetListPlc()
        {
            var dados = await _GeneralApi.GetAsync<List<vmPlc>>($"{routeMgmt}/v1/Plc/getListPlc");

            return dados;
        }

        public async Task<vmPlcSettings> GetSettingsPlc()
        {
            vmPlcSettings? dados = await _GeneralApi.GetAsync<vmPlcSettings>($"{routeMgmt}/v1/Plc/GetSettingsPlc");

            return dados;
        }

        public async Task<bool> UpdateSettingsPlc(vmPlcSettings settings)
        {
            var dados = await _GeneralApi.PostAsync<vmPlcSettings, bool>($"{routeMgmt}/v1/Plc/UpdateSettingsPlc", settings);

            return dados;
        }

        public async Task<bool> TestConnectionPlc()
        {
            bool dados = await _GeneralApi.GetAsync<bool>($"{routeMgmt}/v1/Plc/TestConnectionPlc");

            return dados;
        }

        public async Task<vmPlc> DetailTagInList(int idTag)
        {
            var item = await _GeneralApi.GetAsync<List<vmPlc>>($"{routeMgmt}/v1/Plc/getListPlc");

            return item.FirstOrDefault(x => x.Id == idTag);
        }

        public async Task<bool> AddTagInList(List<vmPlc> plc)
        {
            var item = await _GeneralApi.PostAsync<List<vmPlc>, bool>($"{routeMgmt}/v1/Plc/AddTagInList", plc);
            return item;
        }

        public async Task<bool> UpdateTagInList(vmPlc plc)
        {
            var item = await _GeneralApi.PostAsync<vmPlc, bool>($"{routeMgmt}/v1/Plc/UpdateTagInList", plc);

            return item;
        }

        public async Task<bool> DeleteTagInList(int id)
        {
            var item = await _GeneralApi.DeleteAsync<int, bool>($"{routeMgmt}/v1/Plc/{id}/DeleteTagInList");

            return item;
        }
    }
}
