﻿using Application.Services.Interface;
using Application.Services.Interfaces;
using Application.ViewModels;
using Application.ViewModels.Error;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using System.Reflection;
using System.Text.Json.Serialization;
using UI.Models.ActionsFilter;

namespace UI.Controllers
{
    [AuthorizeToken]
    [AllowAnonymous]
    public class ManagementController : Controller
    {
        private readonly InterCameraService _interCalibrationService;
        private readonly InterConfigurationService _interConfigurationService;
        private readonly InterConfigureOptionsServices _configureOptionsServices;

        public List<vmPlc> vmPlcs = [];

        public ManagementController(InterCameraService interCalibrationService, InterConfigurationService interConfigurationService, InterConfigureOptionsServices configureOptionsServices)
        {
            _interCalibrationService = interCalibrationService;
            _interConfigurationService = interConfigurationService;
            _configureOptionsServices = configureOptionsServices;
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
                return Ok(plcConnection);
            }
            catch (Exception e) { return BadRequest(new ErrorViewModel { Message = e.Message, Response = false }); }
        }

        /// <summary>
        /// Load a list for all tags saved
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

        /// <summary>
        /// open one view for add new tag in list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AddTagInList()
        {
            ViewBag.ListTypeTag = new SelectList(new List<string> { "bool", "int", "double", });
            return PartialView("_AddTagInListPartial");
        }

        /// <summary>
        /// Add a new tag in list
        /// </summary>
        /// <param name="plc"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddTagInList(vmPlc plc)
        {
            try
            {
                List<vmPlc> listPlc = [];

                if (plc.Type == "bool")
                    plc.Value = plc.Value == "1" ? "true" : "false";
                listPlc.Add(plc);

                bool resp = await _interConfigurationService.AddTagInList(listPlc);

                TempDataRequest(resp);
                return Redirect("TagList");
            }
            catch { return Redirect("TagList"); }
        }


        /// <summary>
        /// open a view for edit an existing tag
        /// </summary>
        /// <param name="idTag"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EditTagInList(int idTag)
        {
            ViewBag.ListTypeTag = new SelectList(new List<string> { "bool", "int", "double", });

            vmPlc? item = await _interConfigurationService.DetailTagInList(idTag);

            if (item.Type == "bool")
                item.Value = item.Value.Equals("true") ? "1" : "0";

            return PartialView("_EditTagInListPartial", item);
        }

        /// <summary>
        /// edit an existing tag in the list
        /// </summary>
        /// <param name="plc"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateTagInList(vmPlc plc)
        {
            try
            {
                bool item = await _interConfigurationService.UpdateTagInList(plc);

                TempDataRequest(item);
                return Redirect("TagList");
            }
            catch { return BadRequest(plc); }
        }

        /// <summary>
        /// delete an existing tag in the list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteTagInList(int id)
        {
            try
            {
                bool item = await _interConfigurationService.DeleteTagInList(id);

                TempDataRequest(item);
                return item ? Ok(item) : BadRequest(item);
            }
            catch { return Redirect("TagList"); }
        }

        /// <summary>
        /// Opens a view of the last calibration performed and returns these values
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[AuthorizeRoles("Admin")]
        public async Task<IActionResult> Calibration()
        {

            try
            {
                var data = await _interCalibrationService.GetCalibrationCameraAsync();
                return View(data);
            }
            catch { return View(new vmCalibrationCamera()); }
        }

        /// <summary>
        /// Submit a request to perform a calibration
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[AuthorizeRoles("Admin")]
        public async Task<IActionResult> ExecuteCalibration()
        {
            try
            {
                await _interCalibrationService.ExecuteCalibration();
                return RedirectToAction("", "");
            }
            catch { return RedirectToAction("", ""); }
        }

        /// <summary>
        /// searches for a last performed calibration and return those values
        /// </summary>
        /// <returns></returns>
        //[AuthorizeRoles("User", "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetCalibrationCamera()
        {
            try
            {
                var data = await _interCalibrationService.GetCalibrationCameraAsync();
                return Ok(data);
            }
            catch { return BadRequest(""); }
        }

        /// <summary>
        /// Play a camera antisway
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> VideoCamera()
        {
            try
            {
                //var data = await _interCalibrationService.GetVideoCameraAsync();
                return Ok();
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
        public async Task<IActionResult> Monitoring()
        {
            var data = await _interCalibrationService.GetCalibrationCameraAsync();
            return View(data);
        }

        /// <summary>
        /// Open a view for Plc Settings
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> PlcSettings()
        {
            ViewBag.Driver = await _configureOptionsServices.GetDriversOptions();
            ViewBag.ListCpuType = await _configureOptionsServices.GetCpuTypesOptions();

            vmPlcSettings plcSetting = await _interConfigurationService.GetSettingsPlc();
            return View(plcSetting);
        }

        /// <summary>
        /// Set a new configuration for Plc
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

                return plcSetting ? Redirect("PlcSettings") : BadRequest(plcSetting);
            }
            catch { return BadRequest(false); }
        }

        /// <summary>
        /// Set a new alert message for all requisition's
        /// </summary>
        /// <param name="resp"></param>
        private void TempDataRequest(bool resp)
        {
            if (resp)
                TempData["SuccessMessage"] = "Successfully!";
            else
                TempData["ErrorMessage"] = "Error, not saved!";
        }

        private void TempDataErrorMessage(string message)
        {
            TempData["ErrorMessage"] = message.Trim();
        }

        /// <summary>
        /// Send a new value to tag
        /// </summary>
        /// <param name="vmWrite"></param>
        /// <returns></returns>
        //[HttpPost]
        //public async Task<IActionResult> SetValueToPlc(List<vmWritePlc> vmWrite)
        //{
        //    if (vmWrite == null || !vmWrite.Any())
        //        return BadRequest("No data received.");
        //    try
        //    {
        //        List<vmApiRequestWritePlc> vmApiRequest = [];
        //        foreach (var item in vmWrite)
        //        {
        //            var plcData = await _interConfigurationService.DetailTagInList(item.Id);
        //            vmApiRequest.Add(new vmApiRequestWritePlc
        //            {
        //                AddressPlc = plcData.AddressPlc,
        //                Type = item.Type,
        //                Value = item.Value,
        //            });
        //        }
        //        var resp = await _interConfigurationService.WritePlcAsync(vmApiRequest);

        //        if (resp)
        //            foreach (var item in vmWrite)
        //            {
        //                var plcData = await _interConfigurationService.DetailTagInList(item.Id);
        //                plcData.Value = item.Value;
        //                await _interConfigurationService.UpdateTagInList(plcData);
        //            }

        //        TempDataRequest(resp);

        //        return resp ? Ok(resp) : BadRequest(resp);
        //    }
        //    catch { return BadRequest(false); }
        //}

        [HttpPost]
        public async Task<IActionResult> SetValueToPlc(List<vmWritePlc> vmWrite)
        {
            if (vmWrite == null || !vmWrite.Any())
                return BadRequest("No data received.");
            try
            {
                List<Task<vmApiRequestWritePlc>> listValue = vmWrite.Select(async item =>
                {
                    vmPlc plcData = await _interConfigurationService.DetailTagInList(item.Id);
                    return new vmApiRequestWritePlc
                    {
                        AddressPlc = plcData.AddressPlc,
                        Type = item.Type,
                        Value = item.Value,
                    };
                }).ToList();

                vmApiRequestWritePlc[] vmApiRequest = await Task.WhenAll(listValue);
                bool resp = await _interConfigurationService.WritePlcAsync([.. vmApiRequest]);

                if (resp)
                {
                    IEnumerable<Task> updateList = vmWrite.Select(async item =>
                    {
                        vmPlc plcData = await _interConfigurationService.DetailTagInList(item.Id);
                        plcData.Value = item.Value;
                        await _interConfigurationService.UpdateTagInList(plcData);
                    });
                    await Task.WhenAll(updateList);
                }

                TempDataRequest(resp);
                return resp ? Ok(resp) : BadRequest(resp);
            }
            catch (Exception ex)
            {
                return BadRequest(false);
            }
        }


        /// <summary>
        /// get a last value from tag
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        //[HttpPost]
        //public async Task<IActionResult> GetValueFromPlc(List<string> address)
        //{
        //    if (address.Count == 0)
        //        return BadRequest("No data received.");
        //    try
        //    {
        //        List<vmPlc> list = await _interConfigurationService.GetListPlc();
        //        foreach (var item in address)
        //        {
        //            string resp = await _interConfigurationService.ReadPlcAsync(item);
        //            resp = !String.IsNullOrEmpty(resp) ? resp : "0";

        //            vmPlc? plcToUpdate = list.FirstOrDefault(x => x.AddressPlc == item);

        //            if (plcToUpdate is not null)
        //            {
        //                if (plcToUpdate.Type == "bool")
        //                    plcToUpdate.Value = resp == "1" ? "true" : "false";
        //                else
        //                    plcToUpdate.Value = resp;

        //                await _interConfigurationService.UpdateTagInList(plcToUpdate);
        //            }
        //        }
        //        return Ok(list);
        //    }
        //    catch { return RedirectToAction("TagList"); }
        //}
        [HttpPost]
        public async Task<IActionResult> GetValueFromPlc(List<string> address)
        {
            if (address == null || !address.Any())
                return NotFound("No data received.");
            try
            {
                List<vmPlc> list = await _interConfigurationService.GetListPlc();

                List<Task> tasks = address.Select(async item =>
                {
                    string? resp = await _interConfigurationService.ReadPlcAsync(item);
                    resp = !string.IsNullOrEmpty(resp) ? resp : "0";

                    vmPlc? plcToUpdate = list.FirstOrDefault(x => x.AddressPlc == item);

                    if (plcToUpdate != null)
                    {
                        plcToUpdate.Value = plcToUpdate.Type == "bool" ? (resp == "1" ? "true" : "false") : resp;
                        await _interConfigurationService.UpdateTagInList(plcToUpdate);
                    }
                }).ToList();
                await Task.WhenAll(tasks);

                return Ok(list);
            }
            catch (Exception ex)
            {
                TempDataErrorMessage(ex.Message);
                return RedirectToAction("TagList");
            }
        }

    }
}