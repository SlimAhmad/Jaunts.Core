// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Transaction.Exceptions
{
    public class TransactionProcessingValidationException : Xeption
    {
        public TransactionProcessingValidationException(Xeption innerException)
            : base(message: "Transaction validation error occurred, Please try again.", innerException)
        { }
        public TransactionProcessingValidationException(string message,Xeption innerException)
            : base(message, innerException)
        { }
    }
}
