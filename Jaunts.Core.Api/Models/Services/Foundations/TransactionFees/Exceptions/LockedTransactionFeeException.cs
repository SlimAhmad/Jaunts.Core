// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.TransactionFees.Exceptions
{
    public class LockedTransactionFeeException : Xeption
    {
        public LockedTransactionFeeException(Exception innerException)
            : base(message: "Locked transaction fee  record exception, please try again later.", innerException) { }
        public LockedTransactionFeeException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}
