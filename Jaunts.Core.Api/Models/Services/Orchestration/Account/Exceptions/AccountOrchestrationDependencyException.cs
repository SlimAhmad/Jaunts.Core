// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Orchestration.Account.Exceptions
{
    public class AccountOrchestrationDependencyException : Xeption
    {
        public AccountOrchestrationDependencyException(Xeption innerException)
             : base(message: "Account dependency error occurred, please fix the errors and try again.",
                  innerException)
        { }
    }
}
