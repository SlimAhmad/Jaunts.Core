// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.TransactionFees.Exceptions
{
    public class NotFoundTransactionFeeException : Xeption
    {
        public NotFoundTransactionFeeException(Guid TransactionFeeId)
            : base(message: $"Couldn't find transaction fee  with id: {TransactionFeeId}.") { }
        public NotFoundTransactionFeeException(string message)
            : base(message) { }
    }
}
