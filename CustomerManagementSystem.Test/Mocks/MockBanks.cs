using CommonUtilities.Utilities;
using CustomerManagementSystem.Interfaces;
using CustomerManagementSystem.ViewModels;
using CustomerManagementSystem.ViewModels.Banks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagementSystem.Test.Mocks
{
    public class MockBanks : IBankService
    {
        public async Task<GenericResponse<GetBanksResponseVm>> GetExistingBanks()
        {
           //mock data
           return new GenericResponse<GetBanksResponseVm>()
           {
               IsSuccessful = true,
               ResponseCode = $"0{((int)ResponseCode.Successful).ToString()}",
               ResponseMessage ="succes",
               Data = new GetBanksResponseVm{ 
                    result = new List<Banks>(),
                    hasError = false,
                    timeGenerated = DateTime.Now
               }
           };
        }
    }
}
