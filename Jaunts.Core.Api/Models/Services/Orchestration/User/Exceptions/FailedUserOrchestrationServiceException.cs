// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Orchestration.User.Exceptions
{
    public class FailedUserOrchestrationServiceException : Xeption
    {
        public FailedUserOrchestrationServiceException(Exception innerException)
            : base(message: "Failed User service error occurred, contact support.",
                  innerException)
        { }
    }
}
