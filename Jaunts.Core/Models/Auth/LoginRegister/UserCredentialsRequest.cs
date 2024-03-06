

namespace Jaunts.Core.Api.Models.Auth
{
    /// <summary>
    /// The credentials for  an API client to register on the server 
    /// </summary>
    public class UserCredentialsRequest
    {
        #region Public Properties

        public string Username { get; set; } 
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public Guid Id { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public UserCredentialsRequest()
        {

        }

        #endregion
    }
}
