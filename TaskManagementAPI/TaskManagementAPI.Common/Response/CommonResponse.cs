using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementAPI.Common.Response
{
    public class ServiceResponse<T> : ResponseBase
    {
        public T Data { get; set; }
    }
    public class ResponseBase
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public string? Id { get; set; }
    }
}
