using CommonUtilities.Pagination;
using CommonUtilities.Utilities;
using CustomerManagementSystem.Interfaces;
using CustomerManagementSystem.ViewModels;
using CustomerManagementSystem.ViewModels.Location;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementSystem.Controllers.Location
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocation _locationService;

        public LocationController(ILocation locationService)
        {
            _locationService = locationService;
        }

        [HttpPost("AddLocation")]
        [ProducesResponseType(typeof(GenericResponse<List<GetLocationResponseVm>>), 200)]
        public async Task<IActionResult> AddLocation(List<AddLocationVM> vm)
        {
            if (!ModelState.IsValid)
            {
                var data = new  GenericResponse<List<GetLocationResponseVm>>
                {
                    IsSuccessful = false,
                    ResponseCode = ((int)ResponseCode.INVALID_REQUEST).ToString(),
                    ResponseMessage = "Please pass in the required parameters"
                };
                return BadRequest(data);
            }
            var result = await _locationService.AddLocation(vm);

            return Ok(result);
        }

        [HttpGet("GetLocations")]
        [ProducesResponseType(typeof(GenericResponse<PagedList<GetLocationResponseVm>>), 200)]
        public async Task<IActionResult> GetLocations([FromQuery] PagingVM pagingVM, [FromQuery] GetLocationQueryVm vM)
        {
            var result = await _locationService.GetLocations(pagingVM, vM);

            return Ok(result);
        }

        [HttpPost("UpdateLocation")]
        [ProducesResponseType(typeof(GenericResponse<string>), 200)]
        public async Task<IActionResult> UpdateLocation(List<UpdateLocationVM> model)
        {
            if (!ModelState.IsValid)
            {
                var resp = new GenericResponse<List<GetLocationResponseVm>>
                {
                    IsSuccessful = false,
                    ResponseCode = ((int)ResponseCode.INVALID_REQUEST).ToString(),
                    ResponseMessage = "Please pass in the required parameters"
                };
                return BadRequest(resp);
            }

            var data = await _locationService.UpdateLocation(model);
            if (!data.IsSuccessful && data.ResponseCode == ResponseCode.NotFound.ToString())
            {
                return NotFound(data);
            }
            return Ok(data);
        }

        [HttpGet("GetCountryInfoOnly")]
        [ProducesResponseType(typeof(GenericResponse<GetLocationResponseVm>), 200)]
        public async Task<IActionResult> GetCountryInfoOnly([FromQuery] PagingVM pagingVM)
        {
            var data = await _locationService.GetCountryInfoOnly(pagingVM);

            return Ok(data);
        }
        [HttpGet("GetStateLevelInfoByCountryCode")]
        [ProducesResponseType(typeof(GenericResponse<PagedList<GetStateLevelInfoOnlyVM>>), 200)]
        public async Task<IActionResult> GetStateLevelInfoByCountryCode([FromQuery] PagingVM vm, [FromQuery] string countryCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode))
            {
                var resp = new GenericResponse<List<GetLocationResponseVm>>
                {
                    IsSuccessful = false,
                    ResponseCode = ((int)ResponseCode.INVALID_REQUEST).ToString(),
                    ResponseMessage = "Please pass in a valid countryCode"
                };
                return BadRequest(resp);
            }
            var data = await _locationService.GetStateLevelInfoByCountryCode(vm, countryCode);
            return Ok(data);
        }

        [HttpGet("GetLgaLevelInfoOnly")]
        [ProducesResponseType(typeof(GenericResponse<PagedList<GetLgaLevelInfoOnlyVM>>), 200)]
        public async Task<IActionResult> GetLgaLevelInfoOnly([FromQuery] PagingVM vm, [FromQuery] string countryCode, [FromQuery] string stateCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode) || string.IsNullOrWhiteSpace(stateCode))
            {
                var resp = new GenericResponse<List<GetLocationResponseVm>>
                {
                    IsSuccessful = false,
                    ResponseCode = ((int)ResponseCode.INVALID_REQUEST).ToString(),
                    ResponseMessage = "Please pass in a valid countryCode or stateCode"
                };
                return BadRequest(resp);
            }

            var data = await _locationService.GetLgaLevelInfoOnlyVM(vm, countryCode, stateCode);
            return Ok(data);

        }
    }
}
