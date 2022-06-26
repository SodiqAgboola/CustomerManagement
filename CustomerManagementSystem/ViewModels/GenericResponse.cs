using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementSystem.ViewModels
{
    public class GenericResponse<T>
    {
        public bool IsSuccessful { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public T Data { get; set; }
    }
}
