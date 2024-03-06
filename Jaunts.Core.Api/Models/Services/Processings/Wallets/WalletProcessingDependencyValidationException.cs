// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Wallet.Exceptions
{
    public class WalletProcessingDependencyValidationException : Xeption
    {
        public WalletProcessingDependencyValidationException(Xeption innerException)
            : base(message: "Wallet dependency validation error occurred, Please try again.",
                innerException)
        { }

        public WalletProcessingDependencyValidationException(string message,Xeption innerException)
            : base(message,innerException)
        { }
    }
}
