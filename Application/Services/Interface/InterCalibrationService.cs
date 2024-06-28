using Application.ViewModels;

namespace Application.Services.Interface
{
    public interface InterCalibrationService
    {
        Task<vmCalibrationCamera> GetCalibrationCameraAsync();
        Task<vmCalibrationCamera> SetCalibrationCameraAsync(vmCalibrationCamera calibrationCamera);
    }
}
