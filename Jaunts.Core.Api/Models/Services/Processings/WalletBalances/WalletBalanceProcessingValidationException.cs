// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.WalletBalance.Exceptions
{
    public class WalletBalanceProcessingValidationException : Xeption
    {
        public WalletBalanceProcessingValidationException(Xeption innerException)
            : base(message: "WalletBalance validation error occurred, Please try again.", innerException)
        { }
        public WalletBalanceProcessingValidationException(string message,Xeption innerException)
            : base(message, innerException)
        { }
    }
}
