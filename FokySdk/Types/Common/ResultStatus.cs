namespace FokySdk.Types.Common
{
    /// <summary>
    /// Represents the status of an operation result
    /// </summary>
    public enum ResultStatus
    {
        /// <summary>
        /// The operation was successful
        /// </summary>
        Ok = 200,
        
        /// <summary>
        /// The operation was successful, but the result is not available yet
        /// </summary>
        Created = 201,
        
        /// <summary>
        /// The operation was not successful because parameters are invalid
        /// </summary>
        BadRequest = 400,
        
        /// <summary>
        /// The operation was not successful because the user is not authorized to perform the operation
        /// </summary>
        Unauthorized = 401,
        
        /// <summary>
        /// The operation was not successful because the result is not found
        /// </summary>
        NotFound = 404,
        
        /// <summary>
        /// The operation was not successful because an unexpected error occurred
        /// </summary>
        InternalError = 500
    }
}