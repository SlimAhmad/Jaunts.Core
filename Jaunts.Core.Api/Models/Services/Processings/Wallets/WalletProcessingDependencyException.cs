// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Wallet.Exceptions
{
    public class WalletProcessingDependencyException : Xeption
    {
        public WalletProcessingDependencyException(Xeption innerException)
            : base(message: "Wallet dependency error occurred, Please contact support", innerException)
        { }
        public WalletProcessingDependencyException(string message,Xeption innerException)
            : base(message, innerException)
        { }
    }
}
