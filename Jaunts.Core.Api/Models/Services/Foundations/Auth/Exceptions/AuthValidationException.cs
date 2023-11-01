// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Auth.Exceptions
{
    public class AuthValidationException : Xeption
    {
        public AuthValidationException(Xeption innerException)
            : base(message: "Auth validation errors occurred, please try again.",
                  innerException)
        { }
    }
}
