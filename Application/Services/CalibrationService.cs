using Application.Important_Area;
using Application.Services.Interface;
using Application.ViewModels;
using Infra.RequestApi.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CalibrationService : InterCalibrationService
    {
        private readonly InterGeneralApi _GeneralApi;

        readonly string routePlc = String.Concat(ApiRoutePlc.Link, ApiRoutePlc.Port, ApiRoutePlc.Api);

        public CalibrationService(InterGeneralApi GeneralApi)
        {
            _GeneralApi = GeneralApi;
        }

        public async Task<vmCalibrationCamera> GetCalibrationCameraAsync(string address)
        {
            var dados = await _GeneralApi.GetAsync<dynamic>($"/v1");
            vmCalibrationCamera calibrationCamera = new();
            return calibrationCamera;
        }

        public async Task<vmCalibrationCamera> SetCalibrationCameraAsync(vmCalibrationCamera calibrationCamera)
        {
            var dados = await _GeneralApi.PostAsync<vmCalibrationCamera, dynamic>($"/v1", calibrationCamera);
            
            return calibrationCamera;
        }
    }
}
