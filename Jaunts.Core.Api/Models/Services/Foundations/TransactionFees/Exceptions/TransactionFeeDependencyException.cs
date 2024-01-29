// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.TransactionFees.Exceptions
{
    public class TransactionFeeDependencyException : Xeption
    {
        public TransactionFeeDependencyException(Xeption innerException)
             : base(message: "TransactionFee dependency error occurred, contact support.", innerException) { }
        public TransactionFeeDependencyException(string message,Xeption innerException)
         : base(message, innerException) { }
    }
}
