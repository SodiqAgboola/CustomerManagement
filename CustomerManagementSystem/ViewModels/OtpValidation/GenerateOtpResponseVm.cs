using CustomerManagementSystem.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementSystem.ViewModels.OtpValidation
{
    public class GenerateOtpResponseVm
    {
        public string PhoneNumber { get; set; }
        public DateTime OtpExpiryDate { get; set; }
        public OtpStatusFlag OtpStatusFlag { get; set; }
    }
}
