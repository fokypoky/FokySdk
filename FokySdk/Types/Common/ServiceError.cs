namespace FokySdk.Types.Common
{
    /// <summary>
    /// Represents an error that occurs during a request
    /// </summary>
    public class ServiceError
    {
        /// <summary>
        /// Reason of the error that occurred during a request.
        /// </summary>
        public string Reason { get; set; }
    }
}