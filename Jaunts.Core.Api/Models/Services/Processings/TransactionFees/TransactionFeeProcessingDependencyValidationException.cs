// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.TransactionFee.Exceptions
{
    public class TransactionFeeProcessingDependencyValidationException : Xeption
    {
        public TransactionFeeProcessingDependencyValidationException(Xeption innerException)
            : base(message: "TransactionFee dependency validation error occurred, Please try again.",
                innerException)
        { }

        public TransactionFeeProcessingDependencyValidationException(string message,Xeption innerException)
            : base(message,innerException)
        { }
    }
}
