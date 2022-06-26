using CommonUtilities.Utilities;
using CustomerManagementSystem.Interfaces;
using CustomerManagementSystem.ViewModels;
using CustomerManagementSystem.ViewModels.OtpValidation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementSystem.Controllers.Otp
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtpController : ControllerBase
    {
        private readonly IOtpService _otpService;

        public OtpController(IOtpService otpService)
        {
            this._otpService = otpService;
        }

        [HttpPost("GenerateOtp")]
        [ProducesResponseType(typeof(GenericResponse<List<GenerateOtpResponseVm>>), 200)]
        public async Task<IActionResult> GenerateOtp(GenerateOtpVm vm)
        {
            if (!ModelState.IsValid)
            {
                var data = new GenericResponse<List<GenerateOtpResponseVm>>
                {
                    IsSuccessful = false,
                    ResponseCode = ((int)ResponseCode.INVALID_REQUEST).ToString(),
                    ResponseMessage = "Please pass in the required parameters"
                };
                return BadRequest(data);
            }
            var result = await _otpService.GenerateOtp(vm);

            return Ok(result);
        }
    }
}
