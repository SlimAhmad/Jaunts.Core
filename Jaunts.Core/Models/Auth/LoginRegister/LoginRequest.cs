namespace Jaunts.Core.Api.Models.Auth
{
    /// <summary>
    /// The credentials for an API client to log into the server and receive a token back
    /// </summary>
    public class LoginRequest
    {
        #region Public Properties

        /// <summary>
        /// The users user name or email
        /// </summary>
        public string UsernameOrEmail { get; set; }

        /// <summary>
        /// The users password
        /// </summary>
        public string Password { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LoginRequest()
        {

        }

        #endregion
    }
}
