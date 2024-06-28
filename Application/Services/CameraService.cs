using Application.Important_Area;
using Application.Services.Interface;
using Application.ViewModels;
using Infra.RequestApi.Interface;
using System.Formats.Asn1;

namespace Application.Services
{
    public class CameraService : InterCameraService
    {
        private readonly InterGeneralApi _GeneralApi;

        readonly string routeMgmt = String.Concat(ApiRouteMgmt.Link, ApiRouteMgmt.Port, ApiRouteMgmt.Api);

        public CameraService(InterGeneralApi GeneralApi)
        {
            _GeneralApi = GeneralApi;
        }

        public async Task<vmCalibrationCamera> GetCalibrationCameraAsync()
        {
            var dados = await _GeneralApi.GetAsync<vmCalibrationCamera>($"{routeMgmt}/v1/Camera/GetCalibrationCameraAsync");
            return dados;
        }

        public async Task<bool> ExecuteCalibration()
        {
            var dados = await _GeneralApi.GetAsync<bool>($"{routeMgmt}/v1/Camera/SetCalibrationCameraAsync");

            return dados;
        }

        public async Task<bool> GetVideoCameraAsync()
        {
            throw new NotImplementedException();
        }
    }
}
