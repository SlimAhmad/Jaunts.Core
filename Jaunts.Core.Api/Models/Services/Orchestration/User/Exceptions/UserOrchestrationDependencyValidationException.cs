// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Orchestration.User.Exceptions
{
    public class UserOrchestrationDependencyValidationException : Xeption
    {
        public UserOrchestrationDependencyValidationException(Xeption innerException)
             : base(message: "User dependency validation error occurred, please fix the errors and try again.",
                  innerException)
        { }
    }
}
