// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Orchestration.SignIn.Exceptions
{
    public class SignInOrchestrationServiceException : Xeption
    {
        public SignInOrchestrationServiceException(Xeption innerException)
            : base(message: "SignIn service error occurred, please fix the errors and try again.",
                  innerException)
        { }
    }
}
