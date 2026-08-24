using FokySdk.Types.Common;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace FokySdk.Controller
{
    public static class Controller
    {
        private static ServiceError MapError<T>(ServiceResult<T> response)
        {
            return new ServiceError()
            {
                Code = response.Code,
                Reason = response.Error ?? "Error text is empty",
                Parameters = response.Parameters
            };
        }

        /// <summary>
        /// Map ServiceResult to http response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static ActionResult MapResponse<T>(this ControllerBase controller, ServiceResult<T> response)
        {
            return response.Result switch
            {
                ResultStatus.Ok => controller.Ok(response.Data),
                ResultStatus.Created => controller.Created(),
                ResultStatus.NoContent => controller.NoContent(),
                ResultStatus.PartialContent => controller.StatusCode((int)HttpStatusCode.PartialContent, response.Data),
                ResultStatus.BadRequest => controller.BadRequest(MapError(response)),
                ResultStatus.NotFound => controller.NotFound(MapError(response)),
                ResultStatus.InternalError => controller.StatusCode((int)HttpStatusCode.InternalServerError),
                _ => throw new ArgumentException("Unknown result code")
            };
        }

        /// <summary>
        /// Map paginated ServiceResult to http response with 'x-total-count' header
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static ActionResult MapResponse<T>(this ControllerBase controller, ServiceResult<PaginatedResponse<T>> response)
        {
            if (response.IsOk())
            {
                controller.HttpContext.Response.Headers.TryAdd("x-total-count", response.Data!.TotalCount.ToString());
            }

            return response.Result switch
            {
                ResultStatus.Ok => controller.Ok(response.Data?.Data ?? []),
                ResultStatus.Created => controller.Created(),
                ResultStatus.NoContent => controller.NoContent(),
                ResultStatus.PartialContent => controller.StatusCode((int)HttpStatusCode.PartialContent, response.Data?.Data),
                ResultStatus.BadRequest => controller.BadRequest(MapError(response)),
                ResultStatus.NotFound => controller.NotFound(MapError(response)),
                ResultStatus.InternalError => controller.StatusCode((int)HttpStatusCode.InternalServerError),
                _ => throw new ArgumentException("Unknown result code")
            };
        }
    }
}
