// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.SignIns.Exceptions
{
    public class SignInProcessingDependencyException : Xeption
    {
        public SignInProcessingDependencyException(Xeption innerException)
            : base(message: "SignIn Impression dependency error occurred, please contact support", innerException)
        { }
    }
}
