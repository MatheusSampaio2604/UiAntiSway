using Application.ViewModels;
using Infra.RequestApi;
using Infra.RequestApi.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interface
{
    public interface InterConfigurationService
    {
        Task<vmWritePlc> ReadPlcAsync(string address);
        Task<bool> WritePlcAsync(List<vmApiRequestWritePlc> plcData);
        Task<List<vmPlc>> GetListPlc();
        Task<vmPlc> DetailTagInList(int idTag);
        Task<bool> UpdateTagInList(vmPlc plc);
        Task<bool> AddTagInList(vmPlc plc);

        Task<vmPlcSettings> GetSettingsPlc();
        Task<bool> UpdateSettingsPlc(vmPlcSettings settings);

        Task<bool> TestConnectionPlc();
    }
}
