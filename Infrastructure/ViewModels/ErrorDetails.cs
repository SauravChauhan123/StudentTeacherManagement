using Newtonsoft.Json;
using Serilog;

namespace Infrastructure.ViewModels
{
    /// <summary>
    /// Represents details of an error, including status code and message.
    /// </summary>
    public class ErrorDetails
    {
        #region Properties

        /// <summary>
        /// Gets or sets the HTTP status code associated with the error.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Message { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the error details to a JSON string.
        /// </summary>
        /// <returns>A JSON representation of the error details.</returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Logs the error details using Serilog.
        /// </summary>
        /// <param name="logger">The Serilog logger instance to use for logging.</param>
        public void Log(Serilog.ILogger logger)
        {
            logger.Error("Error occurred with StatusCode: {StatusCode} and Message: {Message}", StatusCode, Message);
        }

        #endregion
    }
}
