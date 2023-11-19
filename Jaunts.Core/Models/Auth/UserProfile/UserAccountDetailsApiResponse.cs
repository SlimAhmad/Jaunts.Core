using System.Collections.Generic;

namespace Jaunts.Core.Api.Models.Auth
{

    /// <summary>
    /// The result of a login request or get user profile details request via API
    /// </summary>
    public class UserAccountDetailsApiResponse
    {
        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The authentication token used to stay authenticated through future requests
        /// </summary>
        /// <remarks>The Token is only provided when called from the login methods</remarks>
        public string Token { get; set; }

        /// <summary>
        /// The users first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The users last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The users username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The users email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The roles associated with the user
        /// </summary>
        public List<string> Role { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool EmailConfirmed { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public UserAccountDetailsApiResponse()
        {

        }

        #endregion

        #region Public Helper Methods

        /// <summary>
        /// Creates a new <see cref="LoginCredentialsDataModel"/>
        /// from this model
        /// </summary>
        /// <returns></returns>
        //public LoginCredentialsDataModel ToLoginCredentialsDataModel()
        //{
        //    return new LoginCredentialsDataModel
        //    {

        //        Email = Email,
        //        FirstName = FirstName,
        //        LastName = LastName,
        //        Username = Username,
        //        Token = Token
        //    };
        //}

        #endregion
    }
}
