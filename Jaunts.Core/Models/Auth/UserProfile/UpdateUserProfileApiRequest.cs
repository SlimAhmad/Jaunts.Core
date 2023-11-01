using System.Collections.Generic;

namespace Jaunts.Core.Api.Models.Auth
{
    /// <summary>
    /// The details to change for a User Profile from an API client call
    /// </summary>
    public class UpdateUserProfileApiRequest
    {
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
        /// The roles associated with the user
        /// </summary>
        public List<string> Role { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfileImage { get; set; }
    }
}
