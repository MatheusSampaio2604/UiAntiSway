using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interface
{
    public interface InterCalibrationService
    {
        Task<vmCalibrationCamera> GetCalibrationCameraAsync(string address);
        Task<vmCalibrationCamera> SetCalibrationCameraAsync(vmCalibrationCamera calibrationCamera);
    }
}
