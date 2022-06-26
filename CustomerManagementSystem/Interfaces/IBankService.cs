using CustomerManagementSystem.ViewModels;
using CustomerManagementSystem.ViewModels.Banks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementSystem.Interfaces
{
    public interface IBankService
    {
        Task<GenericResponse<GetBanksResponseVm>> GetExistingBanks();
    }
}
