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

        /// <summary>
        /// perform a connection test for the current connection
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> TestConnectionPlc()
        {
            try
            {
                bool plcConnection = await _interConfigurationService.TestConnectionPlc();
                return plcConnection ? Ok(plcConnection) : BadRequest(plcConnection);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[AuthorizeRoles("User", "Admin")]
        public async Task<IActionResult> TagList()
        {
            List<vmPlc>? listPlc = await _interConfigurationService.GetListPlc();

            vmPlcs = listPlc;

            return View(listPlc);
        }


        [HttpGet]
        public IActionResult AddTagInList()
        {
            ViewBag.ListTypeTag = new SelectList(new List<string> { "bool", "int", "double", });
            return PartialView("_AddTagInListPartial");
        }

        [HttpPost]
        public IActionResult AddTagInList(vmPlc plc)
        {
            if (!ModelState.IsValid)
            {
                return View(plc);
            }
            var item = _interConfigurationService.AddTagInList(plc);
            return Redirect("TagList");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="idTag"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> DetailTagInList(int idTag)
        {
            ViewBag.ListTypeTag = new SelectList(new List<string> { "bool", "int", "double", });

            vmPlc? item = await _interConfigurationService.DetailTagInList(idTag);
            return PartialView("_DetailTagInListPartial", item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plc"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateTagInList(vmPlc plc)
        {
            bool item = await _interConfigurationService.UpdateTagInList(plc);

            TempDataRequest(item);

            return Redirect("TagList");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[AuthorizeRoles("Admin")]
        public IActionResult Calibration()
        {
            //var data = await _interCalibrationService.GetCalibrationCameraAsync();
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="calibrationCamera"></param>
        /// <returns></returns>
        [HttpPost]
        //[AuthorizeRoles("Admin")]
        public async Task<IActionResult> Calibration(vmCalibrationCamera calibrationCamera)
        {
            await _interCalibrationService.SetCalibrationCameraAsync(calibrationCamera);

            return RedirectToAction("", "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[AuthorizeRoles("User", "Admin")]
        public async Task<vmCalibrationCamera> GetCalibrationCamera()
        {
            var data = await _interCalibrationService.GetCalibrationCameraAsync();
            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[AuthorizeRoles("User", "Admin")]
        public IActionResult Monitoring()
        {
            //var data = await _interCalibrationService.GetCalibrationCameraAsync();
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> PlcSettings()
        {
            ViewBag.ListCpuType = new SelectList(new List<string>
            {
                "S7200", "Logo0BA8", "S7200Smart",
                "S7300", "S7400", "S71200",
                "S71500"
            });

            vmPlcSettings plcSetting = await _interConfigurationService.GetSettingsPlc();
            return View(plcSetting);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plcSettings"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PlcSettings(vmPlcSettings plcSettings)
        {
            try
            {
                bool plcSetting = await _interConfigurationService.UpdateSettingsPlc(plcSettings);

                TempDataRequest(plcSetting);

                return RedirectToAction("PlcSettings", "Management");
            }
            catch (Exception e)
            {
                return BadRequest("Not executed by any error...");
            }
        }

        private void TempDataRequest(bool resp)
        {
            if (resp)
                TempData["SuccessMessage"] = "Successfully!";
            else
                TempData["ErrorMessage"] = "Error, not saved!";
        }

        [HttpPost]
        public async Task<IActionResult> SetValueToPlc(List<vmWritePlc> vmWrite)
        {
            List<vmApiRequestWritePlc> vmApiRequest = new();
            try
            {
                foreach (var item in vmWrite)
                {
                    var plcData = await _interConfigurationService.DetailTagInList(item.Id);
                    vmApiRequest.Add(new vmApiRequestWritePlc
                    {
                        AddressPlc = plcData.AddressPlc,
                        Type = item.Type,
                        Value = item.Value,
                    });
                }
                var resp = await _interConfigurationService.WritePlcAsync(vmApiRequest);

                foreach (var item in vmWrite)
                {
                    vmPlc plc = await _interConfigurationService.DetailTagInList(item.Id);
                    plc.Value = item.Value;
                    var update = await _interConfigurationService.UpdateTagInList(plc);
                }

                TempDataRequest(resp);

                return Ok(resp);
            }
            catch (Exception)
            {
                return Redirect("TagList");
            }
        }

    }
}