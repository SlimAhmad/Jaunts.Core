// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Wallet.Exceptions
{
    public class WalletProcessingValidationException : Xeption
    {
        public WalletProcessingValidationException(Xeption innerException)
            : base(message: "Wallet validation error occurred, Please try again.", innerException)
        { }
        public WalletProcessingValidationException(string message,Xeption innerException)
            : base(message, innerException)
        { }
    }
}
