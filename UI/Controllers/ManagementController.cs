using Application.Services.Interface;
using Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Xml.Linq;
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
                bool plcConnection = false;
                plcConnection = await _interConfigurationService.TestConnectionPlc();
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
        public async Task<IActionResult> AddTagInList(vmPlc plc)
        {
            try
            {
                List<vmPlc> listPlc = [];
                
                if(plc.Type == "bool")
                    plc.Value = plc.Value == "1" ? "true" : "false";
                listPlc.Add(plc);

                await _interConfigurationService.AddTagInList(listPlc);

                List<vmApiRequestWritePlc> vmApiRequest = [new vmApiRequestWritePlc { AddressPlc = plc.AddressPlc, Type = plc.Type, Value = plc.Value }];
                var resp = await _interConfigurationService.WritePlcAsync(vmApiRequest);

                TempDataRequest(resp);

                return Redirect("TagList");
            }
            catch
            {
                return Redirect("TagList");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="idTag"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EditTagInList(int idTag)
        {
            ViewBag.ListTypeTag = new SelectList(new List<string> { "bool", "int", "double", });

            vmPlc? item = await _interConfigurationService.DetailTagInList(idTag);
            return PartialView("_EditTagInListPartial", item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plc"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateTagInList(vmPlc plc)
        {
            try
            {
                bool item = await _interConfigurationService.UpdateTagInList(plc);

                List<vmApiRequestWritePlc> vmApiRequest = [new vmApiRequestWritePlc { AddressPlc = plc.AddressPlc, Type = plc.Type, Value = plc.Value }];
                var resp = await _interConfigurationService.WritePlcAsync(vmApiRequest);

                TempDataRequest(resp);
                return Redirect("TagList");
            }
            catch
            {
                return View(plc);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTagInList(int id)
        {
            try
            {
                bool item = await _interConfigurationService.DeleteTagInList(id);

                TempDataRequest(item);
                return item ? Ok(item) : BadRequest(item);
            }
            catch
            {
                return Redirect("TagList");
            }
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
            try
            {
                await _interCalibrationService.SetCalibrationCameraAsync(calibrationCamera);
                return RedirectToAction("", "");
            }
            catch
            {
                return View(calibrationCamera);
            }
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
            ViewBag.Driver = new SelectList(new List<string> { "SIEMENS", "ROCKWELL" });
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

                if (plcSetting)
                    return Redirect("PlcSettings");
                else
                    return BadRequest(plcSetting);
            }
            catch
            {
                return BadRequest(false);
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
            if (vmWrite == null || !vmWrite.Any())
                return BadRequest("No data received.");

            try
            {
                List<vmApiRequestWritePlc> vmApiRequest = [];

                foreach (var item in vmWrite)
                {
                    var plcData = await _interConfigurationService.DetailTagInList(item.Id);

                    vmApiRequest.Add(new vmApiRequestWritePlc
                    {
                        AddressPlc = plcData.AddressPlc,
                        Type = item.Type,
                        Value = item.Value,
                    });
                    //plcData.Value = item.Value;
                    //await _interConfigurationService.UpdateTagInList(plcData);
                }
                var resp = await _interConfigurationService.WritePlcAsync(vmApiRequest);

                if (resp)
                {
                    foreach (var item in vmWrite)
                    {
                        var plcData = await _interConfigurationService.DetailTagInList(item.Id);
                        plcData.Value = item.Value;
                        await _interConfigurationService.UpdateTagInList(plcData);
                    }
                }
                TempDataRequest(resp);

                return Ok(resp);
            }
            catch
            {
                return Ok(false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetValueFromPlc(List<string> address)
        {
            if (address == null || !address.Any())
                return BadRequest("No data received.");

            try
            {
                List<vmPlc> list = await _interConfigurationService.GetListPlc();


                foreach (var item in address)
                {
                    vmWritePlc resp = await _interConfigurationService.ReadPlcAsync(item);

                    vmPlc? plcToUpdate = list.FirstOrDefault(x => x.Id == resp.Id);

                    if (plcToUpdate != null)
                    {
                        plcToUpdate.Value = resp.Value;
                        await _interConfigurationService.UpdateTagInList(plcToUpdate);
                    }
                }

                return Ok(list);
            }
            catch
            {
                return RedirectToAction("TagList");
            }
        }

    }
}