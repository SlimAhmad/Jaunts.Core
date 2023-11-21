// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Orchestration.SignIn.Exceptions
{
    public class SignInOrchestrationDependencyException : Xeption
    {
        public SignInOrchestrationDependencyException(Xeption innerException)
             : base(message: "SignIn dependency error occurred, please fix the errors and try again.",
                  innerException)
        { }
    }
}
