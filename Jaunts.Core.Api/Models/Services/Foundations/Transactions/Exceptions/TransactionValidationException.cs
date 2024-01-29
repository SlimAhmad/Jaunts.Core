// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Transactions.Exceptions
{
    public class TransactionValidationException : Xeption
    {
        public TransactionValidationException(Xeption innerException)
            : base(message: "Transaction validation error occurred, Please try again.", innerException) { }
        public TransactionValidationException(string message,Xeption innerException)
            : base(message, innerException) { }

    }
}