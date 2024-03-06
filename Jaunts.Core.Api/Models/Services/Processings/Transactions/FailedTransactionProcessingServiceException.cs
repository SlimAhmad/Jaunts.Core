// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Transaction.Exceptions
{
    public class FailedTransactionProcessingServiceException : Xeption
    {
        public FailedTransactionProcessingServiceException(Exception innerException)
            : base(message: "Failed Transaction service occurred, Please contact support", innerException)
        { }

        public FailedTransactionProcessingServiceException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
