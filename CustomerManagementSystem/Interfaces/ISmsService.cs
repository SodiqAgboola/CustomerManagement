using CustomerManagementSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementSystem.Interfaces
{
    public interface ISmsService
    {
        Task<GenericResponse<string>> SendOtpToUser(string phoneNumber, string Otp);
    }
}
