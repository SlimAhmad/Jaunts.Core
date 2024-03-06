// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.TransactionFee.Exceptions
{
    public class TransactionFeeProcessingValidationException : Xeption
    {
        public TransactionFeeProcessingValidationException(Xeption innerException)
            : base(message: "TransactionFee validation error occurred, Please try again.", innerException)
        { }
        public TransactionFeeProcessingValidationException(string message,Xeption innerException)
            : base(message, innerException)
        { }
    }
}
