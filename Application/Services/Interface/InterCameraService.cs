using Application.ViewModels;

namespace Application.Services.Interface
{
    public interface InterCameraService
    {
        Task<vmCalibrationCamera> GetCalibrationCameraAsync();
        Task<bool> ExecuteCalibration();

        Task<bool> GetVideoCameraAsync();
    }
}
