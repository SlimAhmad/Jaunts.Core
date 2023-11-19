// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Orchestration.Role.Exceptions
{
    public class FailedRoleOrchestrationServiceException : Xeption
    {
        public FailedRoleOrchestrationServiceException(Exception innerException)
            : base(message: "Failed Role service error occurred, contact support.",
                  innerException)
        { }
    }
}
