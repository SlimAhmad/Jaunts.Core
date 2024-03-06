// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.WalletBalance.Exceptions
{
    public class WalletBalanceProcessingDependencyValidationException : Xeption
    {
        public WalletBalanceProcessingDependencyValidationException(Xeption innerException)
            : base(message: "WalletBalance dependency validation error occurred, Please try again.",
                innerException)
        { }

        public WalletBalanceProcessingDependencyValidationException(string message,Xeption innerException)
            : base(message,innerException)
        { }
    }
}
