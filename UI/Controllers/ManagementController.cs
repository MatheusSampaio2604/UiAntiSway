using Application.Services.Interface;
using Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UI.Models.ActionsFilter;

namespace UI.Controllers
{
    //[AuthorizeToken]
    public class ManagementController : Controller
    {
        private readonly InterCalibrationService _interCalibrationService;
        private readonly InterConfigurationService _interConfigurationService;

        public List<vmPlc> vmPlcs = [];

        public ManagementController(InterCalibrationService interCalibrationService, InterConfigurationService interConfigurationService)
        {
            _interCalibrationService = interCalibrationService;
            _interConfigurationService = interConfigurationService;
        }

        [HttpGet]
        //[AuthorizeRoles("User", "Admin")]
        public async Task<IActionResult> Configuration()
        {
            List<vmPlc>? listPlc = await _interConfigurationService.GetListPlc();

            vmPlcs = listPlc;
            //ViewBag.ListPlc = listPlc;



            return View(listPlc);
        }

        [HttpGet]
        //[AuthorizeRoles("Admin")]
        public IActionResult Calibration()
        {
            return View();
        }

        [HttpPost]
        //[AuthorizeRoles("Admin")]
        public IActionResult Calibration(vmCalibrationCamera calibrationCamera)
        {
            var item = _interCalibrationService.SetCalibrationCameraAsync(calibrationCamera);

            return RedirectToAction("", "");
        }

        [HttpPost]
        //[AuthorizeRoles("User", "Admin")]
        public async Task<vmCalibrationCamera> GetCalibrationCamera()
        {
            var data = await _interCalibrationService.GetCalibrationCameraAsync("");
            return data;
        }

        [HttpGet]
        //[AuthorizeRoles("User", "Admin")]
        public IActionResult Monitoring()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> PlcSettings()
        {
            ViewBag.ListCpuType = new SelectList(new List<string>
            {
                "S7200",
                "Logo0BA8",
                "S7200Smart",
                "S7300",
                "S7400",
                "S71200",
                "S71500"
            });

            vmPlcSettings plcSetting = await _interConfigurationService.GetSettingsPlc();
            return View(plcSetting);
        }

        [HttpPost]
        public async Task<IActionResult> PlcSettings(vmPlcSettings plcSettings)
        {
            try
            {
                vmPlcSettings plcSetting = await _interConfigurationService.UpdateSettingsPlc(plcSettings);

                TempData["SuccessMessage"] = "Setting saved successfully!";

                return RedirectToAction("PlcSettings", "Management");
            }
            catch (Exception)
            {
                return BadRequest("Not executed by any error...");
            }
        }

        [HttpGet]
        public async Task<IActionResult> TestConnectionPlc()
        {

            try
            {
                bool plcConnection = await _interConfigurationService.TestConnectionPlc();

                return plcConnection ?  Ok(plcConnection) : BadRequest(plcConnection); 
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}