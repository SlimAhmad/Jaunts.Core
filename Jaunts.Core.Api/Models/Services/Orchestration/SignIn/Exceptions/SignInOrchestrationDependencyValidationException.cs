// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Orchestration.SignIn.Exceptions
{
    public class SignInOrchestrationDependencyValidationException : Xeption
    {
        public SignInOrchestrationDependencyValidationException(Xeption innerException)
             : base(message: "SignIn dependency validation error occurred, please fix the errors and try again.",
                  innerException)
        { }
    }
}
