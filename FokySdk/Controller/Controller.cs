using FokySdk.Types.Common;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FokySdk.Controller
{
    public static class Controller
    {
        private static ServiceError MapError<T>(ServiceResult<T> response)
        {
            return new ServiceError() { Reason = response.Error ?? "Error text is empty" };
        }

        /// <summary>
        /// Map ServiceResult to http response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IActionResult MapResponse<T>(this ControllerBase controller, ServiceResult<T> response)
        {
            return response.Result switch
            {
                ResultStatus.Ok => controller.Ok(response.Data),
                ResultStatus.Created => controller.Created(),
                ResultStatus.BadRequest => controller.BadRequest(MapError(response)),
                ResultStatus.NotFound => controller.NotFound(MapError(response)),
                ResultStatus.InternalError => controller.StatusCode((int)HttpStatusCode.InternalServerError),
                _ => throw new ArgumentException("Unknown result code")
            };
        }
    }
}
