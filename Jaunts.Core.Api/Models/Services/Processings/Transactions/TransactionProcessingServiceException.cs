// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Transaction.Exceptions
{
    public class TransactionProcessingServiceException : Xeption
    {
        public TransactionProcessingServiceException(Exception innerException)
            : base(message: "Failed Transaction processing service occurred, Please contact support", innerException)
        { }
        public TransactionProcessingServiceException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
