using Application.ViewModels;

namespace Application.Services.Interface
{
    public interface InterConfigurationService
    {
        Task<string> ReadPlcAsync(string address);
        Task<bool> WritePlcAsync(List<vmApiRequestWritePlc> plcData);
        Task<List<vmPlc>> GetListPlc();
        Task<vmPlc> DetailTagInList(int idTag);
        Task<bool> UpdateTagInList(vmPlc plc);
        Task<bool> DeleteTagInList(int id);

        Task<bool> AddTagInList(List<vmPlc> plc);

        Task<vmPlcSettings> GetSettingsPlc();
        Task<bool> UpdateSettingsPlc(vmPlcSettings settings);

        Task<bool> TestConnectionPlc();
    }
}
