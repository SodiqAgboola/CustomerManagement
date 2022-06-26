using CommonUtilities.Pagination;
using CommonUtilities.Utilities;
using CustomerManagementSystem.HelperMethods;
using CustomerManagementSystem.Interfaces;
using CustomerManagementSystem.Models;
using CustomerManagementSystem.ViewModels;
using CustomerManagementSystem.ViewModels.Location;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementSystem.Services
{
    public class LocationService : ILocation
    {
        private readonly IAsyncRepository<Location> _repository;
        private readonly ILogger<LocationService> _logger;

        public LocationService(IAsyncRepository<Location> repository, ILogger<LocationService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<GenericResponse<List<GetLocationResponseVm>>> AddLocation(List<AddLocationVM> bvm)
        {
            _logger.LogInformation($"Received request to create location.");
            try
            {
                var locations = new List<Location>();
                foreach (var location in bvm)
                {
                    var pp = location.SetObjectProperties(new Location());
                    locations.Add(pp);
                }
                var data = await _repository.AddRangeAsync(locations);

                var resp = data.SetObjectPropertiesFromList(new List<GetLocationResponseVm>());

                return new GenericResponse<List<GetLocationResponseVm>> { IsSuccessful = true, ResponseCode = ((int)ResponseCode.Successful).ToString(),
                    ResponseMessage = (ResponseCode.Successful).ToString(), Data = resp };
            }
            catch(Exception ex)
            {
                _logger.LogError($"AddLocation: An exception occured. Messg: {ex.Message} - StackTrace: {ex.StackTrace}");
                return new GenericResponse<List<GetLocationResponseVm>> { IsSuccessful = false, ResponseCode = $"0{((int)ResponseCode.ProcessingError).ToString()}", 
                    ResponseMessage = (ResponseCode.ProcessingError).ToString()};
            }
        }

        public async Task<GenericResponse<PagedList<GetCountryOnlyResponseVM>>> GetCountryInfoOnly(PagingVM vm)
        {
            try
            {
                var query = _repository.GetAll();

                var data = new PagedList<Location>(query, vm.PageNumber, vm.PageSize, false);

                var result = data.ConvertPagedListTo<Location, GetCountryOnlyResponseVM>();

                return new GenericResponse<PagedList<GetCountryOnlyResponseVM>> { IsSuccessful = true, ResponseCode = $"0{((int)ResponseCode.Successful).ToString()}",
                    ResponseMessage = (ResponseCode.Successful).ToString(),
                    Data = result };
            }
            catch(Exception ex)
            {
                _logger.LogError($"GetCountryInfoOnly: An exception occured. Messg: {ex.Message} - StackTrace: {ex.StackTrace}");
                return new GenericResponse<PagedList<GetCountryOnlyResponseVM>> { IsSuccessful = false, ResponseCode = $"0{((int)ResponseCode.ProcessingError).ToString()}",
                    ResponseMessage = (ResponseCode.ProcessingError).ToString() };
            }
        }

        public async Task<GenericResponse<PagedList<GetLgaLevelInfoOnlyVM>>> GetLgaLevelInfoOnlyVM(PagingVM vm, string countryCode, string stateCode)
        {
            try
            {
                var result = new GenericResponse<PagedList<GetLgaLevelInfoOnlyVM>>() {
                    IsSuccessful = true,
                    ResponseCode = $"0{((int)ResponseCode.Successful).ToString()}",
                    ResponseMessage = (ResponseCode.Successful).ToString(),
                };
                var query = _repository.GetAll().Where(x => x.CountryCode == countryCode && x.StateCode == stateCode);

                var data = new PagedList<Location>(query, vm.PageNumber, vm.PageSize, false);
                var b1 = data.ConvertPagedListTo<Location, GetLgaLevelInfoOnlyVM>();
                result.Data = b1;

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetLgaLevelInfoOnlyVM: An exception occured. Messg: {ex.Message} - StackTrace: {ex.StackTrace}");
                return new GenericResponse<PagedList<GetLgaLevelInfoOnlyVM>>
                {
                    IsSuccessful = false,
                    ResponseCode = $"0{((int)ResponseCode.ProcessingError).ToString()}",
                    ResponseMessage = (ResponseCode.ProcessingError).ToString()
                };
            }
        }

        public async Task<GenericResponse<GetLocationResponseVm>> GetLocationById(int Id)
        {
            try
            {
                var result = new GenericResponse<GetLocationResponseVm>() {
                    IsSuccessful = true,
                    ResponseCode = $"0{((int)ResponseCode.Successful).ToString()}",
                    ResponseMessage = (ResponseCode.Successful).ToString()
                };
                var query = _repository.GetAll().FirstOrDefault(x => x.Id == Id);

                if (query is null)
                {
                    return new GenericResponse<GetLocationResponseVm>() {
                        IsSuccessful = false,
                        ResponseCode = $"0{((int)ResponseCode.NotFound).ToString()}",
                        ResponseMessage = $"No location with id: {Id} exists"
                    };
                }

                var b1 = query.SetObjectProperties(new GetLocationResponseVm());
                result.Data = b1;

                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError($"GetLocationById: An exception occured. Messg: {ex.Message} - StackTrace: {ex.StackTrace}");
                return new GenericResponse<GetLocationResponseVm>
                {
                    IsSuccessful = false,
                    ResponseCode = $"0{((int)ResponseCode.ProcessingError).ToString()}",
                    ResponseMessage = (ResponseCode.ProcessingError).ToString()
                };
            }
        }

        public async Task<GenericResponse<PagedList<GetLocationResponseVm>>> GetLocations(PagingVM vm, GetLocationQueryVm bvm)
        {
            try
            {
                var query = _repository.GetAll().GetData(bvm);

                var data = new PagedList<Location>(query, vm.PageNumber, vm.PageSize, false);

                var result = data.ConvertPagedListTo<Location, GetLocationResponseVm>();

                return new GenericResponse<PagedList<GetLocationResponseVm>> {
                    IsSuccessful = true,
                    ResponseCode = $"0{((int)ResponseCode.Successful).ToString()}",
                    ResponseMessage = (ResponseCode.Successful).ToString(),
                    Data = result };
            }

            catch(Exception ex)
            {
                _logger.LogError($"GetLocations: An exception occured. Messg: {ex.Message} - StackTrace: {ex.StackTrace}");
                return new GenericResponse<PagedList<GetLocationResponseVm>>
                {
                    IsSuccessful = false,
                    ResponseCode = $"0{((int)ResponseCode.ProcessingError).ToString()}",
                    ResponseMessage = (ResponseCode.ProcessingError).ToString()
                };
            }
        }

        public async Task<GenericResponse<PagedList<GetStateLevelInfoOnlyVM>>> GetStateLevelInfoByCountryCode(PagingVM vm, string countryCode)
        {
            try
            {
                var result = new GenericResponse<PagedList<GetStateLevelInfoOnlyVM>>() {
                    IsSuccessful = true,
                    ResponseCode = $"0{((int)ResponseCode.Successful).ToString()}",
                    ResponseMessage = (ResponseCode.Successful).ToString(),
                };
                var query = _repository.GetAll().Where(x => x.CountryCode == countryCode);

                var data = new PagedList<Location>(query, vm.PageNumber, vm.PageSize, false);
                var b1 = data.ConvertPagedListTo<Location, GetStateLevelInfoOnlyVM>();
                result.Data = b1;

                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError($"GetStateLevelInfoByCountryCode: An exception occured. Messg: {ex.Message} - StackTrace: {ex.StackTrace}");
                return new GenericResponse<PagedList<GetStateLevelInfoOnlyVM>>
                {
                    IsSuccessful = false,
                    ResponseCode = $"0{((int)ResponseCode.ProcessingError).ToString()}",
                    ResponseMessage = (ResponseCode.ProcessingError).ToString()
                };
            }
        }

        public async Task<GenericResponse<string>> UpdateLocation(List<UpdateLocationVM> model)
        {
            try
            {
                var result = new GenericResponse<string>() {
                    IsSuccessful = true,
                    ResponseCode = $"0{((int)ResponseCode.Successful).ToString()}",
                    ResponseMessage = (ResponseCode.Successful).ToString(),
                };
                var ids = model.Select(x => x.Id).ToList();
                var locationList = _repository.GetAll().Where(x => ids.Contains(x.Id)).ToList();
                if (locationList.Count < 1)
                {
                    return new GenericResponse<string>()
                    {
                        IsSuccessful = false,
                        ResponseCode = $"0{((int)ResponseCode.NotFound).ToString()}",
                        ResponseMessage = $"No Location found for Ids specified",
                        Data = "failed"
                    };
                }

                foreach (var location in locationList)
                {
                    var updateLocation = model.FirstOrDefault(x => x.Id == location.Id);

                    updateLocation.SetObjectProperties(location);
                }
                await _repository.UpdateRangeAsync(locationList);

                result.Data = "Success";

                return result;

            }
            catch(Exception ex)
            {
                _logger.LogError($"UpdateLocation: An exception occured. Messg: {ex.Message} - StackTrace: {ex.StackTrace}");
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
