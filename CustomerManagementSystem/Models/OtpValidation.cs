using CustomerManagementSystem.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementSystem.Models
{
    public class OtpValidation
    {
        public int Id { get; set; }
        public string Otp { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime OtpExpiryDate { get; set; }
        public OtpStatusFlag OtpStatusFlag { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
