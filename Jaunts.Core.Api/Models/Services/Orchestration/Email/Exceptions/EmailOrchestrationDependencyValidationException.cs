// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Orchestration.Email.Exceptions
{
    public class EmailOrchestrationDependencyValidationException : Xeption
    {
        public EmailOrchestrationDependencyValidationException(Xeption innerException)
             : base(message: "Email dependency validation error occurred, please fix the errors and try again.",
                  innerException)
        { }
    }
}
