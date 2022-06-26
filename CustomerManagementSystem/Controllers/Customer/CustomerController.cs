using CommonUtilities.Pagination;
using CommonUtilities.Utilities;
using CustomerManagementSystem.Interfaces;
using CustomerManagementSystem.ViewModels;
using CustomerManagementSystem.ViewModels.Customer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementSystem.Controllers.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomer _customerService;

        public CustomerController(ICustomer customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("OnboardCustomer")]
        [ProducesResponseType(typeof(GenericResponse<List<GetCustomerResponseVm>>), 200)]
        public async Task<IActionResult> CreateCustomer(List<AddCustomerVm> vm)
        {
            if (!ModelState.IsValid)
            {
                var data = new GenericResponse<List<GetCustomerResponseVm>>
                {
                    IsSuccessful = false,
                    ResponseCode = ((int)ResponseCode.INVALID_REQUEST).ToString(),
                    ResponseMessage = "Please pass in the required parameters"
                };
                return BadRequest(data);
            }
            var result = await _customerService.CreateCustomer(vm);

            return Ok(result);
        }

        [HttpGet("GetAllExistingCustomers")]
        [ProducesResponseType(typeof(GenericResponse<PagedList<GetCustomerResponseVm>>), 200)]
        public async Task<IActionResult> GetCustomers([FromQuery] PagingVM pagingVM, [FromQuery] GetCustomerQueryVm vM)
        {
            var result = await _customerService.GetCustomers(pagingVM, vM);

            return Ok(result);
        }

        [HttpPost("UpdateCustomerInfo")]
        [ProducesResponseType(typeof(GenericResponse<string>), 200)]
        public async Task<IActionResult> UpdateCustomer(List<UpdateCustomerVm> model)
        {
            if (!ModelState.IsValid)
            {
                var resp = new GenericResponse<List<UpdateCustomerVm>>
                {
                    IsSuccessful = false,
                    ResponseCode = ((int)ResponseCode.INVALID_REQUEST).ToString(),
                    ResponseMessage = "Please pass in the required parameters"
                };
                return BadRequest(resp);
            }

            var data = await _customerService.UpdateCustomer(model);
            if (!data.IsSuccessful && data.ResponseCode == ResponseCode.NotFound.ToString())
            {
                return NotFound(data);
            }
            return Ok(data);
        }
    }
}
