// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.SignIns.Exceptions
{
    public class SignInProcessingDependencyValidationException : Xeption
    {
        public SignInProcessingDependencyValidationException(Xeption innerException)
            : base(message: "SignIn dependency validation error occurred, please try again.",
                innerException)
        { }

        public SignInProcessingDependencyValidationException(string message,Xeption innerException)
            : base(message,innerException)
        { }
    }
}
