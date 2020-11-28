using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ByHandDeliveryApi.GenericResponses
{
    public static class ResponseExtension
    {
        public static IActionResult ToHttpResponse<TModel>(this IGenericResponse<TModel> response)
        {
            var status = HttpStatusCode.OK;
            response.StatusCode = (Int32)(HttpStatusCode.OK);

            if (response.HasError)
            {
                if (!string.IsNullOrEmpty(response.Message) && response.Message.Equals("Invalid credentials."))
                {
                    status = HttpStatusCode.Unauthorized;
                    response.StatusCode = (Int32)(HttpStatusCode.Unauthorized);
                }
                else
                {
                    status = HttpStatusCode.InternalServerError;
                    response.StatusCode = (Int32)(HttpStatusCode.InternalServerError);
                }
            }
            else if (response.Result == null)
            {
                status = HttpStatusCode.NotFound;
                response.StatusCode = (Int32)(HttpStatusCode.NotFound);
            }

            return new ObjectResult(response)
            {
                StatusCode = (Int32)status,
            };
        }
    }
}
