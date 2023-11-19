// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Orchestration.Email.Exceptions
{
    public class EmailOrchestrationDependencyException : Xeption
    {
        public EmailOrchestrationDependencyException(Xeption innerException)
             : base(message: "Email dependency error occurred, please fix the errors and try again.",
                  innerException)
        { }
    }
}
