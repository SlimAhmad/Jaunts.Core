// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Auth.Exceptions
{
    public class AuthDependencyValidationException : Xeption
    {
        public AuthDependencyValidationException(Xeption innerException)
            : base(message: "Auth dependency validation occurred, please try again.", innerException)
        { }
    }
}
