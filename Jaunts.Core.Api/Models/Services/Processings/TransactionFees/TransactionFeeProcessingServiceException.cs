// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.TransactionFee.Exceptions
{
    public class TransactionFeeProcessingServiceException : Xeption
    {
        public TransactionFeeProcessingServiceException(Exception innerException)
            : base(message: "Failed TransactionFee processing service occurred, Please contact support", innerException)
        { }
        public TransactionFeeProcessingServiceException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
