// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Orchestration.Transaction.Exceptions
{
    public class TransactionOrchestrationDependencyException : Xeption
    {
        public TransactionOrchestrationDependencyException(Xeption innerException)
             : base(message: "Transaction dependency error occurred, Please fix the errors and try again.",
                  innerException)
        { }
    }
}
