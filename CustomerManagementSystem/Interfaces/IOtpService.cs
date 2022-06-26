using CustomerManagementSystem.ViewModels;
using CustomerManagementSystem.ViewModels.OtpValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementSystem.Interfaces
{
    public interface IOtpService
    {
        Task<GenericResponse<GenerateOtpResponseVm>> GenerateOtp(GenerateOtpVm model);
        Task<GenericResponse<string>> ValidateOtp(ValidateOtpVm model);
    }
}
