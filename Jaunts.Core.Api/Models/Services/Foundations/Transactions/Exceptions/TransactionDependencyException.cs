// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Transactions.Exceptions
{
    public class TransactionDependencyException : Xeption
    {
        public TransactionDependencyException(Xeption innerException)
             : base(message: "Transaction dependency error occurred, contact support.", innerException) { }
        public TransactionDependencyException(string message,Xeption innerException)
         : base(message, innerException) { }
    }
}
