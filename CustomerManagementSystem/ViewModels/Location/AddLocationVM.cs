using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementSystem.ViewModels.Location
{
    public class AddLocationVM
    {
        [Required]
        public string CountryName { get; set; }
        [Required]
        public string CountryCode { get; set; }
        [Required]
        public string StateName { get; set; }
        [Required]
        public string StateCode { get; set; }
        [Required]
        public string LgaName { get; set; }
        [Required]
        public string LgaCode { get; set; }
    }
}
