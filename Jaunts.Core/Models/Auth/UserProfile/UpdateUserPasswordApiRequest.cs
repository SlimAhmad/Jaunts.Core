namespace Jaunts.Core.Api.Models.Auth
{
    /// <summary>
    /// The details to change for a Users Password from an API client call
    /// </summary>
    public class UpdateUserPasswordApiRequest
    {
        #region Public Properties

        /// <summary>
        /// The users current password
        /// </summary>
        public string CurrentPassword { get; set; }

        /// <summary>
        /// The users new password
        /// </summary>
        public string NewPassword { get; set; }

        #endregion
    }
}
