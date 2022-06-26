using CommonUtilities.Utilities;
using CustomerManagementSystem.Interfaces;
using CustomerManagementSystem.ViewModels;
using CustomerManagementSystem.ViewModels.Banks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagementSystem.Services
{
    public class BankService : IBankService
    {
        private readonly string getAllBanksEndpoint = Startup.StaticConfig["GetAllBanksEndpoint"];
        private readonly string getAllBanksBaseUrl = Startup.StaticConfig["GetAllBanksBaseUrl"];
        private readonly string subscriptionKey = Startup.StaticConfig["SubscriptionKey"];

        private readonly ILogger<BankService> _logger;

        public BankService(ILogger<BankService> logger)
        {
            this._logger = logger;
        }

        public async Task<GenericResponse<GetBanksResponseVm>> GetExistingBanks()
        {
            _logger.LogInformation("received request to fecth existing banks record");
            try
            {
                var existingBankRecords = await FecthExistingBanks();

                if(existingBankRecords == null)
                {
                    _logger.LogInformation("GetExistingBanks request failed");
                    return new GenericResponse<GetBanksResponseVm>()
                    {
                        IsSuccessful = false,
                        ResponseCode =$"0{((int)ResponseCode.ProcessingError).ToString()}",
                        ResponseMessage = $"An error occured while getting existingBanks."
                    };
                }
                else if(!existingBankRecords.hasError && existingBankRecords.result.Count > 0)
                {
                    _logger.LogInformation("GetExistingBanks request succesful");

                    return new GenericResponse<GetBanksResponseVm>()
                    {
                        IsSuccessful = true,
                        ResponseCode = $"0{((int)ResponseCode.Successful).ToString()}",
                        ResponseMessage = $"Success",
                        Data = existingBankRecords
                    };
                }
                else
                {
                    _logger.LogInformation("GetExistingBanks request failed");
                    return new GenericResponse<GetBanksResponseVm>()
                    {
                        IsSuccessful = false,
                        ResponseCode = $"0{((int)ResponseCode.ProcessingError).ToString()}",
                        ResponseMessage = $"{existingBankRecords.errorMessage}",
                    };
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"GetExistingBanks: An exception occured. Messg: {ex.Message} - StackTrace: {ex.StackTrace}");
                return new GenericResponse<GetBanksResponseVm>
                {
                    IsSuccessful = false,
                    ResponseCode = $"0{((int)ResponseCode.ProcessingError).ToString()}",
                    ResponseMessage = (ResponseCode.ProcessingError).ToString()
                };
            }
        }

        public async Task<GetBanksResponseVm> FecthExistingBanks()
        {
            var result = new GetBanksResponseVm();
            try {
                using var httpclient = new HttpClient();
                httpclient.BaseAddress = new Uri($"{getAllBanksBaseUrl}");
                httpclient.DefaultRequestHeaders.Clear();
                httpclient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                var res = await httpclient.GetAsync($"{getAllBanksEndpoint}");

                if (!res.IsSuccessStatusCode)
                {
                    var error = await res.Content.ReadAsStringAsync();

                     result = JsonConvert.DeserializeObject<GetBanksResponseVm>(error);
                    _logger.LogError($"FecthExistingBanks: ResponseCode: {res.StatusCode.ToString()}");
                    _logger.LogError($"FecthExistingBanks Response: {res.ReasonPhrase} : Content: {res.Content.ReadAsStringAsync()}");

                    return result;
                }

                var resString = await res.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<GetBanksResponseVm>(resString);
                _logger.LogInformation($"FecthExistingBanks Response: {res.ReasonPhrase} : Content: {res.Content.ReadAsStringAsync()}");

                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Exception occured while Fecthing ExistingBanks Request. Messg: {ex.Message} : StackTrace: {ex.StackTrace} ");
                return result;
            }
           
        }
    }
}
