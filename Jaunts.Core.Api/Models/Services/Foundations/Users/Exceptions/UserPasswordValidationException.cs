using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Users.Exceptions
{
    public class UserPasswordValidationException : Xeption
    {
        public UserPasswordValidationException()
            : base(message: "User password is invalid, please try again")
        { }
        public UserPasswordValidationException(string message,Exception innerException)
        : base(message,innerException) { }
    }
}
