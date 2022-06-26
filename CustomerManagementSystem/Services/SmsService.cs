using CommonUtilities.Utilities;
using CustomerManagementSystem.Interfaces;
using CustomerManagementSystem.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementSystem.Services
{
    public class SmsService : ISmsService
    {
        private readonly ILogger<SmsService> _logger;
        public SmsService(ILogger<SmsService> logger)
        {
            _logger = logger;
        }

        public async Task<GenericResponse<string>> SendOtpToUser(string phoneNumber, string Otp)
        {
            _logger.LogInformation($"Sending otp to user: {phoneNumber}");

            var text = Startup.StaticConfig["Otptext"];
            var OtpExpiryInMin = Convert.ToDouble(Startup.StaticConfig["OtpExpiryInMin"]);

            text = text.Replace("%otp%", $"{Otp}").Replace("%min%", $"{OtpExpiryInMin}");

            phoneNumber = FormatPhoneNumber(phoneNumber);
            if (phoneNumber == null)
            {
                _logger.LogError($"Invalid PhoneLength - phoneNum: {phoneNumber}");
                return new GenericResponse<string>
                {
                    IsSuccessful = false,
                    ResponseCode = $"0{((int)ResponseCode.ProcessingError).ToString()}",
                    ResponseMessage = $"Invalid PhoneLength - phoneNum: {phoneNumber}"
                };
            }

            //mock succesful response
            _logger.LogInformation($"otp succesfully sent to user: {phoneNumber}");
            return new GenericResponse<string>
            {
                IsSuccessful = true,
                ResponseCode = $"0{((int)ResponseCode.Successful).ToString()}",
                ResponseMessage = (ResponseCode.Successful).ToString(),
            };
        }

        public string FormatPhoneNumber(string phoneNumber)
        {
            var formatPhone = phoneNumber;
            if (phoneNumber.Length > 11)
            {
                return null;
            }
            if (phoneNumber.Length == 11)
            {
                formatPhone = phoneNumber.Substring(1, 10);
            }
            var formatedPhone = $"{"234"}{formatPhone}";
            return formatedPhone;
        }
    }
}
