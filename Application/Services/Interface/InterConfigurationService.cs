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
        Task<vmWriteReadPlc> ReadPlcAsync(string address);
        Task<bool> WritePlcAsync(vmWriteReadPlc plcData);
        Task<List<vmPlc>> GetListPlc();

        Task<vmPlcSettings> GetSettingsPlc();
        Task<vmPlcSettings> UpdateSettingsPlc(vmPlcSettings settings);

        Task<bool> TestConnectionPlc();
    }
}
