// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Orchestration.Transaction.Exceptions
{
    public class TransactionOrchestrationDependencyValidationException : Xeption
    {
        public TransactionOrchestrationDependencyValidationException(Xeption innerException)
             : base(message: "Transaction dependency validation error occurred, Please fix the errors and try again.",
                  innerException)
        { }
    }
}
