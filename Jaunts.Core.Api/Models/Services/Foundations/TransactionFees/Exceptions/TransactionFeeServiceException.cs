// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.TransactionFees.Exceptions
{
    public class TransactionFeeServiceException : Xeption
    {
        public TransactionFeeServiceException(Xeption innerException)
            : base(message: "TransactionFee service error occurred, contact support.", innerException) { }
        public TransactionFeeServiceException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}