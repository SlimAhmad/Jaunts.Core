// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Transactions.Exceptions
{
    public class TransactionServiceException : Xeption
    {
        public TransactionServiceException(Xeption innerException)
            : base(message: "Transaction service error occurred, contact support.", innerException) { }
        public TransactionServiceException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}