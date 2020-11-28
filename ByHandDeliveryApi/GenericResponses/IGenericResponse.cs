using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace  ByHandDeliveryApi.GenericResponses
{
    public interface IGenericResponse<TModel>
    {
        string Message { get; set; }
        TModel Result { get; set; }
        bool HasError { get; set; }
        int StatusCode { get; set; }
    }
}
