using System.Text.Json.Serialization;

namespace Jaunts.Core.Api
{
    /// <summary>
    /// The Response for all Web API calls made
    /// </summary>
    public class ApiResponse
    {
        #region Public Properties

        /// <summary>
        /// Indicates if the API call was successful
        /// </summary>
        public bool Successful => ErrorMessage == null;

        /// <summary>
        /// The error message for a failed API call
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// The API Response object
        /// </summary>
        [JsonIgnore]
        public object Response { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ApiResponse()
        {

        }

        #endregion
    }

    /// <summary>
    /// The Response for all Web API calls made
    /// with a specific type of known Response
    /// </summary>
    /// <typeparam name="T">The specific type of server Response</typeparam>
    public class ApiResponse<T> : ApiResponse
    {
        /// <summary>
        /// The API Response object as T
        /// </summary>
        [JsonPropertyName("Response")]
        public new T Response { get => (T)base.Response; set => base.Response = value; }
    }
}
