using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ByHandDeliveryApi.GenericResponses
{
    public class GenericResponse<TModel> :IGenericResponse<TModel>
    {
        public string Message { get; set; }
        public TModel Result { get; set; }
        public bool HasError { get; set; }
        public int StatusCode { get; set; }

    }
}
