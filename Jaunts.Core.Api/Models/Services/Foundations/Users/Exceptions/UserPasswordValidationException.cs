using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Users.Exceptions
{
    public class UserPasswordValidationException : Xeption
    {
        public UserPasswordValidationException()
            : base(message: "User password is invalid")
        { }
        public UserPasswordValidationException(string message)
        : base(message) { }
    }
}
