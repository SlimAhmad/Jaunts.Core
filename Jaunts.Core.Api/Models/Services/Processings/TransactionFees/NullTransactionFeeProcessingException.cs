// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.TransactionFee.Exceptions
{
    public class NullTransactionFeeProcessingException : Xeption
    {
        public NullTransactionFeeProcessingException()
            : base(message: "TransactionFee is null.")
        { }
        public NullTransactionFeeProcessingException(string message)
            : base(message)
        { }
    }
}
