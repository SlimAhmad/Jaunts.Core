// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Transactions.Exceptions
{
    public class FailedTransactionServiceException : Xeption
    {
        public FailedTransactionServiceException(Exception innerException)
            : base(message: "Failed Transaction service error occurred, contact support.",
                  innerException)
        { }
        public FailedTransactionServiceException(string message,Exception innerException)
          : base(message,
                innerException)
        { }
    }
}
