// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.TransactionFees.Exceptions
{
    public class NullTransactionFeeException : Xeption
    {
        public NullTransactionFeeException() : base(message: "The transaction fee is null.") { }
        public NullTransactionFeeException(string message) : base(message) { }
    }
}
