// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.SignIns.Exceptions
{
    public class SignInProcessingValidationException : Xeption
    {
        public SignInProcessingValidationException(Xeption innerException)
            : base(message: "SignIn validation error occurred, please try again.", innerException)
        { }
    }
}
