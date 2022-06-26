using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementSystem.ViewModels.OtpValidation
{
    public class ValidateOtpVm
    {
        [Required]
        public string Otp { get; set; }
        [Phone]
        [StringLength(11, ErrorMessage = "Maximum phone number length is 11.")]
        public string PhoneNumber { get; set; }
    }
}
