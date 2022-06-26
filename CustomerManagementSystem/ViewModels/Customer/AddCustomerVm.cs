using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementSystem.ViewModels.Customer
{
    public class AddCustomerVm
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [Phone]
        [StringLength(11, ErrorMessage = "Maximum phone number length is 11.")]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(6, ErrorMessage = "Maximum otp length is 6.")]
        public string Otp { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string StateOfResidence { get; set; }
        [Required]
        public string LGA { get; set; }
    }
}
