using System.Collections.Generic;

namespace Jaunts.Core.Api.Models.Auth
{
    /// <summary>
    /// The credentials for an API client to log into the server and receive a token back
    /// </summary>
    public class LoginCredentialsRequest
    {
        #region Public Properties
        /// <summary>
        /// The id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The new first name, or null to leave unchanged
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The new last name, or null to leave unchanged
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The new email, or null to leave unchanged
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The new user name, or null to leave unchanged
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The Token, or null to leave unchanged
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// The roles associated with the user
        /// </summary>
        public List<string> Role { get; set; }



        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LoginCredentialsRequest()
        {

        }

        #endregion
    }
}
