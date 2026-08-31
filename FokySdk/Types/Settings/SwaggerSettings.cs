namespace FokySdk.Types.Settings
{
    /// <summary>
    /// Represents settings used to configure Swagger documentation in an application.
    /// </summary>
    public class SwaggerSettings
    {
        /// <summary>
        /// Gets or sets the name of the service to be used in Swagger documentation.
        /// </summary>
        /// <remarks>
        /// This property is used to specify the title of the Swagger documentation
        /// for the application, providing context about the service being described.
        /// </remarks>
        public string ServiceName { get; set; }

        /// <summary>
        /// Gets or sets the version of the service to be displayed in Swagger documentation.
        /// </summary>
        /// <remarks>
        /// This property is used to specify the version of the API or service, allowing consumers
        /// to distinguish between different versions of the service available in the Swagger documentation.
        /// </remarks>
        public string ServiceVersion { get; set; }

        /// <summary>
        /// Sets the version of service from env variable 'SERVICE_VERSION'. Sets 'DEV' value if 'SERVICE_VERSION' is null
        /// </summary>
        public void SetVersionFromEnvironment()
        {
            ServiceVersion = Environment.GetEnvironmentVariable("SERVICE_VERSION") ?? "DEV";
        }

        /// <summary>
        /// Sets the version of service from env variable 'SERVICE_NAME'. Sets 'SERVICE' value if 'SERVICE_VERSION' is null
        /// </summary>
        public void SetNameFromEnvironment()
        {
            ServiceName = Environment.GetEnvironmentVariable("SERVICE_NAME") ?? "SERVICE";
        }

        /// <summary>
        /// Get settings from environment variables 'SERVICE_NAME' and 'SERVICE_VERSION'. If environment variables are null then use default values:
        /// 'SERVICE_NAME' = 'SERVICE'
        /// 'SERVICE_VERSION' = 'DEV'
        /// </summary>
        /// <returns>SwaggerSettings</returns>
        public static SwaggerSettings GetFromEnvironment()
        {
            var settings = new SwaggerSettings();
            settings.SetNameFromEnvironment();
            settings.SetVersionFromEnvironment();

            return settings;
        }
    }
}