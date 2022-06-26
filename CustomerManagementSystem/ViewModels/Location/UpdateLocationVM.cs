using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementSystem.ViewModels.Location
{
    public class UpdateLocationVM
    {
        [Required]
        [Range(minimum: 1, maximum: int.MaxValue)]
        public int Id { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public string LgaName { get; set; }
        public string LgaCode { get; set; }
    }
}
