using CommonUtilities.Pagination;
using CommonUtilities.Utilities;
using CustomerManagementSystem.HelperMethods;
using CustomerManagementSystem.Interfaces;
using CustomerManagementSystem.Models;
using CustomerManagementSystem.ViewModels;
using CustomerManagementSystem.ViewModels.Customer;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementSystem.Services
{
    public class CustomerService : ICustomer
    {

        private readonly IAsyncRepository<Customer> _repository;
        private readonly ILogger<CustomerService> _logger;
        private readonly IOtpService _otpService;

        public CustomerService(IAsyncRepository<Customer> repository, ILogger<CustomerService> logger,
            IOtpService otpService)
        {
            _repository = repository;
            _logger = logger;
            this._otpService = otpService;
        }
        public async Task<GenericResponse<List<GetCustomerResponseVm>>> CreateCustomer(List<AddCustomerVm> bvm)
        {
            var resp2 = bvm.Select(x => new
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                PhoneNumber = x.PhoneNumber,
                StateOfResidence = x.StateOfResidence,
                Otp = x.Otp
            }).ToList();
            _logger.LogInformation($"Received request to create customer. payload: {JsonConvert.SerializeObject(resp2)}");
            try
            {
                var customers = new List<Customer>();
                foreach (var item in bvm)
                {
                    //validate unique phoneNum/user
                    var phoneNumUnique = _repository.GetAll().Where(x => x.PhoneNumber == item.PhoneNumber).FirstOrDefault();
                    if(phoneNumUnique is not null)
                    {
                        return new GenericResponse<List<GetCustomerResponseVm>>
                        {
                            IsSuccessful = false,
                            ResponseCode = $"0{((int)ResponseCode.ProcessingError).ToString()}",
                            ResponseMessage = $"user with PhoneNumber: {phoneNumUnique.PhoneNumber} exists"
                        };
                    }

                    //validate otp
                    var otpValid = await _otpService.ValidateOtp(new ViewModels.OtpValidation.ValidateOtpVm
                    { Otp = item.Otp.Trim(), PhoneNumber = item.PhoneNumber });

                    if (!otpValid.IsSuccessful)
                    {
                        return new GenericResponse<List<GetCustomerResponseVm>>
                        {
                            IsSuccessful = false,
                            ResponseCode = $"0{((int)ResponseCode.ProcessingError).ToString()}",
                            ResponseMessage = otpValid.ResponseMessage
                        };
                    }

                    var pp = item.SetObjectProperties(new Customer());
                    pp.PhoneNumberVerified = true;
                    pp.Password = Security.ComputeHash(item.Password);
                    customers.Add(pp);
                }

                var data = await _repository.AddRangeAsync(customers);

                var resp = data.SetObjectPropertiesFromList(new List<GetCustomerResponseVm>());

                return new GenericResponse<List<GetCustomerResponseVm>>
                {
                    IsSuccessful = true,
                    ResponseCode = $"0{((int)ResponseCode.Successful).ToString()}",
                    ResponseMessage = (ResponseCode.Successful).ToString(),
                    Data = resp
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateCustomer: An exception occured. Messg: {ex.Message} - StackTrace: {ex.StackTrace}");
                return new GenericResponse<List<GetCustomerResponseVm>>
                {
                    IsSuccessful = false,
                    ResponseCode = $"0{((int)ResponseCode.ProcessingError).ToString()}",
                    ResponseMessage = (ResponseCode.ProcessingError).ToString()
                };
            }
        }

        public async Task<GenericResponse<PagedList<GetCustomerResponseVm>>> GetCustomers(PagingVM vm, GetCustomerQueryVm bvm)
        {
            try
            {
                var query = _repository.GetAll().GetData(bvm);

                var data = new PagedList<Customer>(query, vm.PageNumber, vm.PageSize, false);

                var result = data.ConvertPagedListTo<Customer, GetCustomerResponseVm>();

                return new GenericResponse<PagedList<GetCustomerResponseVm>>
                {
                    IsSuccessful = true,
                    ResponseCode = $"0{((int)ResponseCode.Successful).ToString()}",
                    ResponseMessage = (ResponseCode.Successful).ToString(),
                    Data = result
                };
            }

            catch (Exception ex)
            {
                _logger.LogError($"GetCustomers: An exception occured. Messg: {ex.Message} - StackTrace: {ex.StackTrace}");
                return new GenericResponse<PagedList<GetCustomerResponseVm>>
                {
                    IsSuccessful = false,
                    ResponseCode = $"0{((int)ResponseCode.ProcessingError).ToString()}",
                    ResponseMessage = (ResponseCode.ProcessingError).ToString()
                };
            }
        }

        public async Task<GenericResponse<string>> UpdateCustomer(List<UpdateCustomerVm> model)
        {
            try
            {
                var result = new GenericResponse<string>()
                {
                    IsSuccessful = true,
                    ResponseCode = ((int)ResponseCode.Successful).ToString(),
                    ResponseMessage = (ResponseCode.Successful).ToString(),
                };
                var ids = model.Select(x => x.Id).ToList();
                var customerList = _repository.GetAll().Where(x => ids.Contains(x.Id)).ToList();
                if (customerList.Count < 1)
                {
                    return new GenericResponse<string>()
                    {
                        IsSuccessful = false,
                        ResponseCode = $"0{((int)ResponseCode.NotFound).ToString()}",
                        ResponseMessage = $"No customer found for Ids specified",
                        Data = "failed"
                    };
                }

                foreach (var item in customerList)
                {
                    var updateCustomer = model.FirstOrDefault(x => x.Id == item.Id);

                    updateCustomer.SetObjectProperties(item);
                }
                await _repository.UpdateRangeAsync(customerList);

                result.Data = "Success";

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"UpdateCustomer: An exception occured. Messg: {ex.Message} - StackTrace: {ex.StackTrace}");
                return new GenericResponse<string>
                {
                    IsSuccessful = false,
                    ResponseCode = $"0{((int)ResponseCode.ProcessingError).ToString()}",
                    ResponseMessage = (ResponseCode.ProcessingError).ToString()
                };
            }
        }
    }
}
