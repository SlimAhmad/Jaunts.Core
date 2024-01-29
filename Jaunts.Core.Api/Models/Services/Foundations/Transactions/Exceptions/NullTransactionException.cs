// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Transactions.Exceptions
{
    public class NullTransactionException : Xeption
    {
        public NullTransactionException() : base(message: "The Transaction is null.") { }
        public NullTransactionException(string message) : base(message) { }
    }
}
