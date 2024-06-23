using Application.Services.Interface;
using Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using UI.Models.ActionsFilter;

namespace UI.Controllers
{
    [AuthorizeToken]
    public class GerenciamentoController : Controller
    {
        private readonly InterCalibrationService _interCalibrationService;

        public GerenciamentoController(InterCalibrationService interCalibrationService)
        {
            _interCalibrationService = interCalibrationService;
        }

        [HttpGet]
        public IActionResult Configuration()
        {
            return View();
        }

        [HttpGet]
        [AuthorizeRoles("Admin")]
        public IActionResult Calibration()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Monitoring()
        {
            return View();
        }

        [HttpPost]
        public async Task<vmCalibrationCamera> GetCalibrationCamera()
        {
            var data = await _interCalibrationService.GetCalibrationCameraAsync("");
            return data;
        }
    }
}