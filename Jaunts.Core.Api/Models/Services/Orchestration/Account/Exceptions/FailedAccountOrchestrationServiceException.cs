// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Orchestration.Account.Exceptions
{
    public class FailedAccountOrchestrationServiceException : Xeption
    {
        public FailedAccountOrchestrationServiceException(Exception innerException)
            : base(message: "Failed Account service error occurred, contact support.",
                  innerException)
        { }
    }
}
