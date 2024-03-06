// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Transaction.Exceptions
{
    public class TransactionProcessingDependencyValidationException : Xeption
    {
        public TransactionProcessingDependencyValidationException(Xeption innerException)
            : base(message: "Transaction dependency validation error occurred, Please try again.",
                innerException)
        { }

        public TransactionProcessingDependencyValidationException(string message,Xeption innerException)
            : base(message,innerException)
        { }
    }
}
