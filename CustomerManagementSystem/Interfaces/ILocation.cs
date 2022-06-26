using CommonUtilities.Pagination;
using CustomerManagementSystem.ViewModels;
using CustomerManagementSystem.ViewModels.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementSystem.Interfaces
{
    public interface ILocation
    {
        Task<GenericResponse<List<GetLocationResponseVm>>> AddLocation(List<AddLocationVM> bvm);
        Task<GenericResponse<string>> UpdateLocation(List<UpdateLocationVM> model);
        Task<GenericResponse<PagedList<GetLocationResponseVm>>> GetLocations(PagingVM vm, GetLocationQueryVm bvm);
        Task<GenericResponse<GetLocationResponseVm>> GetLocationById(int Id);
        Task<GenericResponse<PagedList<GetCountryOnlyResponseVM>>> GetCountryInfoOnly(PagingVM vm);
        Task<GenericResponse<PagedList<GetStateLevelInfoOnlyVM>>> GetStateLevelInfoByCountryCode(PagingVM vm, string countryCode);
        Task<GenericResponse<PagedList<GetLgaLevelInfoOnlyVM>>> GetLgaLevelInfoOnlyVM(PagingVM vm, string countryCode, string stateCode);

    }
}
