using Jaunts.Core.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Energy.Wallet.WebApi
{
    /// <summary>
    /// The authorization policy for token-based authentication
    /// </summary>
    public class AuthorizeTokenAttribute : AuthorizeAttribute
    {

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public AuthorizeTokenAttribute()
        {

        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="policy">policy claims</param>
        public AuthorizeTokenAttribute(string policy) : base(policy) { }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="permission">permission claims</param>
        public AuthorizeTokenAttribute(Permissions permission)
        {
            // Add the JWT bearer authentication scheme
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;

            Permissions = permission;
        }

        #endregion

        public Permissions Permissions
        {
            get
            {
                return !string.IsNullOrEmpty(Policy)
                    ? PolicyNameHelper.GetPermissionsFrom(Policy)
                    : Permissions.None;
            }
            set
            {
                Policy = value != Permissions.None
                    ? PolicyNameHelper.GeneratePolicyNameFor(value)
                    : string.Empty;
            }
        }
    }
}
