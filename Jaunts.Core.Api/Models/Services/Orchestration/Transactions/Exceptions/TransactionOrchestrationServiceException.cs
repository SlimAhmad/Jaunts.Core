﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Orchestration.Transaction.Exceptions
{
    public class TransactionOrchestrationServiceException : Xeption
    {
        public TransactionOrchestrationServiceException(Xeption innerException)
            : base(message: "Transaction service error occurred, Please fix the errors and try again.",
                  innerException)
        { }
    }
}
