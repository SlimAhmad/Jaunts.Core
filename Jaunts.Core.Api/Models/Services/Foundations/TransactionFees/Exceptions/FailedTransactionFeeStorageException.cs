// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.TransactionFees.Exceptions
{
    public class FailedTransactionFeeStorageException : Xeption
    {
        public FailedTransactionFeeStorageException(Exception innerException)
            : base(message: "Failed Transaction fee  storage error occurred, please contact support.", innerException)
        { }
        public FailedTransactionFeeStorageException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
