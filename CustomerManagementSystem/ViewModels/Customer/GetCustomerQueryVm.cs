using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementSystem.ViewModels.Customer
{
    public class GetCustomerQueryVm
    {
        public int? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string StateOfResidence { get; set; }
        public string LGA { get; set; }
        public bool? PhoneNumberVerified { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
