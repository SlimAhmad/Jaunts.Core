namespace Jaunts.Core.Routes
{
    /// <summary>
    /// The relative routes to all Api calls in the server
    /// </summary>
    public static class ApiRoutes
    {
        #region Login / Register

        /// <summary>
        /// The route to the Register Api method
        /// </summary>
        public const string Register = "api/register";

        /// <summary>
        /// The route to the Login Api method
        /// </summary>
        public const string Login = "api/login";

        /// <summary>
        /// The route to the VerifyEmail Api method
        /// </summary>
        /// <remarks>
        ///     Pass the userId and emailToken as get parameters.
        ///     i.e. /api/verify/email?userId=...&emailToken=...
        /// </remarks>
        public const string VerifyEmail = "api/verify/email";

        public const string ResetPassword = "api/password/reset";

        public const string ForgotPassword = "api/password/forgotten";

        public const string ConfirmEmail = "api/confirm/email";

        public const string Enable2FA = "api/enable/2fa";

        public const string LoginWithOTP = "api/login/otp";

        #endregion

        #region User Profile

        /// <summary>
        /// The route to the GetUserProfile Api method
        /// </summary>
        public const string GetUserProfile = "api/user/profile";

        /// <summary>
        /// The route to the UpdateUserProfile Api method
        /// </summary>
        public const string UpdateUserProfile = "api/user/profile/update";

        /// <summary>
        /// The route to the UpdateUserPassword Api method
        /// </summary>
        public const string UpdateUserPassword = "api/user/password/update";

        #endregion

      
    }
}
