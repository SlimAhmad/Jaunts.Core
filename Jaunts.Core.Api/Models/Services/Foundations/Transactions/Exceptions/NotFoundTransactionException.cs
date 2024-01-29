// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Transactions.Exceptions
{
    public class NotFoundTransactionException : Xeption
    {
        public NotFoundTransactionException(Guid TransactionId)
            : base(message: $"Couldn't find Transaction with id: {TransactionId}.") { }
        public NotFoundTransactionException(string message)
            : base(message) { }
    }
}
