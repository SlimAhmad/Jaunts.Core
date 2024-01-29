// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.TransactionFees.Exceptions
{
    public class FailedTransactionFeeServiceException : Xeption
    {
        public FailedTransactionFeeServiceException(Exception innerException)
            : base(message: "Failed TransactionFee service error occurred, contact support.",
                  innerException)
        { }
        public FailedTransactionFeeServiceException(string message,Exception innerException)
          : base(message,
                innerException)
        { }
    }
}
