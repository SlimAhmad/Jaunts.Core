// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Transaction.Exceptions
{
    public class TransactionProcessingDependencyException : Xeption
    {
        public TransactionProcessingDependencyException(Xeption innerException)
            : base(message: "Transaction dependency error occurred, Please contact support", innerException)
        { }
        public TransactionProcessingDependencyException(string message,Xeption innerException)
            : base(message, innerException)
        { }
    }
}
