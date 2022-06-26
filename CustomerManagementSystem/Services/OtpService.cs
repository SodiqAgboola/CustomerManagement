using CommonUtilities.Utilities;
using CustomerManagementSystem.HelperMethods;
using CustomerManagementSystem.Interfaces;
using CustomerManagementSystem.Models;
using CustomerManagementSystem.ViewModels;
using CustomerManagementSystem.ViewModels.Enums;
using CustomerManagementSystem.ViewModels.OtpValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementSystem.Services
{
    public class OtpService : IOtpService
    {
        private readonly IAsyncRepository<OtpValidation> _repository;
        private readonly ILogger<OtpService> _logger;
        private readonly ISmsService _smsService;

        public OtpService(IAsyncRepository<OtpValidation> repository, ILogger<OtpService> logger,
            ISmsService smsService)
        {
            _repository = repository;
             _logger = logger;
            this._smsService = smsService;
        }

        public async Task<GenericResponse<GenerateOtpResponseVm>> GenerateOtp(GenerateOtpVm model)
        {
            try
            {
                _logger.LogInformation($"Received request to generate otp for user: {model.PhoneNumber}");

                var otp = OtpGenerator.NewOTP();
                if (string.IsNullOrWhiteSpace(otp))
                {
                    return new GenericResponse<GenerateOtpResponseVm>()
                    {
                        IsSuccessful = false,
                        ResponseCode = ((int)ResponseCode.ProcessingError).ToString(),
                        ResponseMessage = $"Unable to generate otp at the moment."
                    };
                }

                var OtpExpiryInMin = Convert.ToDouble(Startup.StaticConfig["OtpExpiryInMin"]);
                var dateTime = DateTime.Now;
                var expiryDate = dateTime.AddMinutes(OtpExpiryInMin);

                //insert otp record
                var otpValidation = new OtpValidation
                {
                    Otp = otp.Trim(),
                    PhoneNumber = model.PhoneNumber,
                    OtpExpiryDate = expiryDate,
                    OtpStatusFlag = (int)OtpStatusFlag.OtpCreated
                };
                var data = await _repository.AddAsync(otpValidation);
                var resp2 = data.SetObjectProperties(new GenerateOtpResponseVm());

                //send otp to user
                var resp = await _smsService.SendOtpToUser(model.PhoneNumber, otp);
                if (!resp.IsSuccessful)
                {
                    _logger.LogError($"failed to send otp to user: {model.PhoneNumber}");
                    return new GenericResponse<GenerateOtpResponseVm>
                    {
                        IsSuccessful = false,
                        ResponseCode = $"0{((int)ResponseCode.ProcessingError).ToString()}",
                        ResponseMessage = resp.ResponseMessage
                    };
                }

                return new GenericResponse<GenerateOtpResponseVm>()
                {
                    IsSuccessful = true,
                    ResponseCode = $"0{((int)ResponseCode.Successful).ToString()}",
                    ResponseMessage = (ResponseCode.Successful).ToString(),
                    Data = resp2
                };
            }
            catch(Exception ex)
            {
                _logger.LogError($"GenerateOtp: An exception occured. Messg: {ex.Message} - StackTrace: {ex.StackTrace}");
                return new GenericResponse<GenerateOtpResponseVm>
                {
                    IsSuccessful = false,
                    ResponseCode = $"0{((int)ResponseCode.ProcessingError).ToString()}",
                    ResponseMessage = (ResponseCode.ProcessingError).ToString()
                };
            }
        }

        public async Task<GenericResponse<string>> ValidateOtp(ValidateOtpVm model)
        {
            var result = new GenericResponse<string>();
            try
            {
                var errorMessg = string.Empty;
                var otpValid = _repository.GetAll().FirstOrDefault(x => x.PhoneNumber == model.PhoneNumber && x.Otp == model.Otp);
                if (otpValid is null)
                {
                    _logger.LogError($"otp record not found. phoneNum: {model.PhoneNumber} - otp: {model.Otp} ");
                    return new GenericResponse<string>
                    {
                        IsSuccessful = false,
                        ResponseCode = $"0{((int)ResponseCode.ProcessingError).ToString()}",
                        ResponseMessage = "otp not found for phoneNumber, please generate otp"
                    };
                }

                switch (otpValid.OtpStatusFlag)
                {
                    case OtpStatusFlag.OtpUsed:
                        errorMessg = "otp Used. please generate new otp";
                        result.IsSuccessful = false;
                        result.ResponseCode = ((int)ResponseCode.ProcessingError).ToString();
                        result.ResponseMessage = errorMessg;

                        return result;
                    case OtpStatusFlag.OtpExpired:
                        errorMessg = "otp has expired. please generate new otp";
                        result.IsSuccessful = false;
                        result.ResponseCode = $"0{((int)ResponseCode.ProcessingError).ToString()}";
                        result.ResponseMessage = errorMessg;

                        return result;
                    default:
                        break;
                }

                //check if otp is fine
                if (otpValid.Otp != model.Otp)
                {
                    _logger.LogError($"Invalid otp. phoneNum: {model.PhoneNumber} - otp: {model.Otp} ");

                    errorMessg = "Invalid otp.";
                    result.IsSuccessful = false;
                    result.ResponseCode = $"0{((int)ResponseCode.ProcessingError).ToString()}";
                    result.ResponseMessage = errorMessg;

                    return result;
                }

                //check if otp hasnt expired
                if (DateTime.Now > otpValid.OtpExpiryDate)
                {
                    _logger.LogError($"otp expired. phoneNum: {model.PhoneNumber} - otp: {model.Otp} ");

                    otpValid.OtpStatusFlag = OtpStatusFlag.OtpExpired;
                    await _repository.UpdateAsync(otpValid);

                    errorMessg = "otp has expired. please generate new otp.";
                    result.IsSuccessful = false;
                    result.ResponseCode = $"0{((int)ResponseCode.ProcessingError).ToString()}";
                    result.ResponseMessage = errorMessg;

                    return result;
                }

                //otp valid

                otpValid.OtpStatusFlag = OtpStatusFlag.OtpUsed;
                await _repository.UpdateAsync(otpValid);

                errorMessg = "otp validated successfully";
                result.IsSuccessful = true;
                result.ResponseCode = $"0{((int)ResponseCode.Successful).ToString()}";
                result.ResponseMessage = errorMessg;
                result.Data = "success";

                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError($"ValidateOtp: An exception occured. Messg: {ex.Message} - StackTrace: {ex.StackTrace}");
                return new GenericResponse<string>
                {
                    IsSuccessful = false,
                    ResponseCode = $"0{((int)ResponseCode.ProcessingError).ToString()}",
                    ResponseMessage = (ResponseCode.ProcessingError).ToString()
                };
            }
        }
    }
}
