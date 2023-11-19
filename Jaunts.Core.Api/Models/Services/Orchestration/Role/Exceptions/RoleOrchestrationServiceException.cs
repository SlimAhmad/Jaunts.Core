// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Orchestration.Role.Exceptions
{
    public class RoleOrchestrationServiceException : Xeption
    {
        public RoleOrchestrationServiceException(Xeption innerException)
            : base(message: "Role service error occurred, please fix the errors and try again.",
                  innerException)
        { }
    }
}
