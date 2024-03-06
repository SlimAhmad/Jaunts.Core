// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.TransactionFee.Exceptions
{
    public class InvalidTransactionFeeProcessingException : Xeption
    {
        public InvalidTransactionFeeProcessingException()
            : base(message: "Invalid TransactionFee, Please correct the errors and try again.") 
        { }
        public InvalidTransactionFeeProcessingException(string message)
            : base(message)
        { }
    }
}
