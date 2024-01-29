// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Transactions.Exceptions
{
    public class FailedTransactionStorageException : Xeption
    {
        public FailedTransactionStorageException(Exception innerException)
            : base(message: "Failed Transaction storage error occurred, Please contact support.", innerException)
        { }
        public FailedTransactionStorageException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
