using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementSystem.ViewModels.Banks
{
    public class GetBanksResponseVm
    {
        public List<Banks> result { get; set; }
        public string errorMessage { get; set; } 
        public List<string> errorMessages { get; set; }
        public bool hasError { get; set; }
        public DateTime timeGenerated { get; set; }
    }

    public class Banks
    {
        public string bankName { get; set; }
        public string bankCode { get; set; }
    }
}
