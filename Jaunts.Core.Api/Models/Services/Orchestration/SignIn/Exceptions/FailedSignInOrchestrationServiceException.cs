// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Orchestration.SignIn.Exceptions
{
    public class FailedSignInOrchestrationServiceException : Xeption
    {
        public FailedSignInOrchestrationServiceException(Exception innerException)
            : base(message: "Failed SignIn service error occurred, contact support.",
                  innerException)
        { }
    }
}
