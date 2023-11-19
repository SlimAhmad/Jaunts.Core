// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Orchestration.User.Exceptions
{
    public class UserOrchestrationDependencyException : Xeption
    {
        public UserOrchestrationDependencyException(Xeption innerException)
             : base(message: "User dependency error occurred, please fix the errors and try again.",
                  innerException)
        { }
    }
}
