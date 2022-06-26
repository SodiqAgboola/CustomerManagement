using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementSystem.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string StateName { get; set; } 
        public string StateCode { get; set; }
        public string LgaName { get; set; }
        public string LgaCode { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
