// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Orchestration.Transaction.Exceptions
{
    public class FailedTransactionOrchestrationServiceException : Xeption
    {
        public FailedTransactionOrchestrationServiceException(Exception innerException)
            : base(message: "Failed Transaction service error occurred, contact support.",
                  innerException)
        { }
    }
}
