// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Orchestration.Jwt.Exceptions
{
    public class FailedJwtOrchestrationServiceException : Xeption
    {
        public FailedJwtOrchestrationServiceException(Exception innerException)
            : base(message: "Failed Jwt service error occurred, contact support.",
                  innerException)
        { }
    }
}
