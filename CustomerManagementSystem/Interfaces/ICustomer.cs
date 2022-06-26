using CommonUtilities.Pagination;
using CustomerManagementSystem.ViewModels;
using CustomerManagementSystem.ViewModels.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementSystem.Interfaces
{
    public interface ICustomer
    {
        Task<GenericResponse<List<GetCustomerResponseVm>>> CreateCustomer(List<AddCustomerVm> bvm);
        Task<GenericResponse<PagedList<GetCustomerResponseVm>>> GetCustomers(PagingVM vm, GetCustomerQueryVm bvm);
        Task<GenericResponse<string>> UpdateCustomer(List<UpdateCustomerVm> model);
    }
}
