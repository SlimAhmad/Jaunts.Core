// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Orchestration.Account.Exceptions
{
    public class AccountOrchestrationServiceException : Xeption
    {
        public AccountOrchestrationServiceException(Xeption innerException)
            : base(message: "Account service error occurred, please fix the errors and try again.",
                  innerException)
        { }
    }
}
