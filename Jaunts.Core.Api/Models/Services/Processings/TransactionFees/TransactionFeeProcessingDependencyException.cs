// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.TransactionFee.Exceptions
{
    public class TransactionFeeProcessingDependencyException : Xeption
    {
        public TransactionFeeProcessingDependencyException(Xeption innerException)
            : base(message: "TransactionFee dependency error occurred, Please contact support", innerException)
        { }
        public TransactionFeeProcessingDependencyException(string message,Xeption innerException)
            : base(message, innerException)
        { }
    }
}
