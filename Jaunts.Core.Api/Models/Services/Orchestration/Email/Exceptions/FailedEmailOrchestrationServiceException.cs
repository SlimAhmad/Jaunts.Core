// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Orchestration.Email.Exceptions
{
    public class FailedEmailOrchestrationServiceException : Xeption
    {
        public FailedEmailOrchestrationServiceException(Exception innerException)
            : base(message: "Failed Email service error occurred, contact support.",
                  innerException)
        { }
    }
}
