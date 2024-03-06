// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.TransactionFee.Exceptions
{
    public class FailedTransactionFeeProcessingServiceException : Xeption
    {
        public FailedTransactionFeeProcessingServiceException(Exception innerException)
            : base(message: "Failed TransactionFee service occurred, Please contact support", innerException)
        { }

        public FailedTransactionFeeProcessingServiceException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
