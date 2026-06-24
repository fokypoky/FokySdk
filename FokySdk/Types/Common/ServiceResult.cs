namespace FokySdk.Types.Common
{
    /// <summary>
    /// Command or query result
    /// </summary>
    /// <typeparam name="T">Operating entity type</typeparam>
    public sealed class ServiceResult<T>
    {
        /// <summary>
        /// Result status
        /// </summary>
        public ResultStatus Result { get; set; }
        
        /// <summary>
        /// Command or query result data
        /// </summary>
        public T? Data { get; set; }
        
        /// <summary>
        /// Additional information of result
        /// </summary>
        public string? Message { get; set; }
        
        /// <summary>
        /// Error message
        /// </summary>
        public string? Error { get; set; }
        
        /// <summary>
        /// Check if result is successful
        /// </summary>
        /// <returns></returns>
        public bool IsOk()
        {
            return Result == ResultStatus.Ok;
        }

        /// <summary>
        /// Create a successful result
        /// </summary>
        /// <param name="entity">Result entity</param>
        /// <param name="message">Additional information</param>
        /// <returns></returns>
        public static ServiceResult<T> Ok(T entity, string? message = null)
        {
            return new ServiceResult<T>()
            {
                Result = ResultStatus.Ok,
                Data = entity,
                Message = message
            };
        }
        
        /// <summary>
        /// Create an error result that means that the command or query contains invalid parameters
        /// </summary>
        /// <param name="reason">Reason of error</param>
        /// <returns></returns>
        public static ServiceResult<T> BadRequest(string reason)
        {
            return new ServiceResult<T>()
            {
                Result = ResultStatus.BadRequest,
                Error = reason
            };
        }

        /// <summary>
        /// Create an error result that means that an expected command or query result not found
        /// </summary>
        /// <param name="reason">Reason of error</param>
        /// <returns></returns>
        public static ServiceResult<T> NotFound(string reason)
        {
            return new ServiceResult<T>()
            {
                Result = ResultStatus.NotFound,
                Error = reason
            };
        }

        /// <summary>
        /// Create an error result that means that the user is not authorized to perform the command or query
        /// </summary>
        /// <param name="reason">Reason of error</param>
        /// <returns></returns>
        public static ServiceResult<T> Unauthorized(string reason)
        {
            return new ServiceResult<T>()
            {
                Result = ResultStatus.Unauthorized,
                Error = reason
            };
        }
        
        /// <summary>
        /// Create an error result that means that an unexpected error occurred
        /// </summary>
        /// <param name="reason">Reason of error</param>
        /// <returns></returns>
        public static ServiceResult<T> InternalError(string reason)
        {
            return new ServiceResult<T>()
            {
                Result = ResultStatus.InternalError,
                Error = reason
            };
        }

        /// <summary>
        /// Create an error result from another error result
        /// </summary>
        /// <param name="error">Another error result</param>
        /// <typeparam name="TResult">Type of another error result</typeparam>
        /// <typeparam name="TError">Target type</typeparam>
        /// <returns></returns>
        public static ServiceResult<TResult> FromErrorResult<TResult, TError>(ServiceResult<TError> error)
        {
            return new ServiceResult<TResult>()
            {
                Result = error.Result,
                Error = error.Error,
                Message = error.Message
            };
        }

        /// <summary>
        /// Create an error result from another error result
        /// </summary>
        /// <param name="error">Type of another error result</param>
        /// <returns></returns>
        public static ServiceResult<T> FromErrorResult(ServiceResult<T> error)
        {
            return new ServiceResult<T>()
            {
                Result = error.Result,
                Error = error.Error,
                Message = error.Message
            };
        }
    }
}
