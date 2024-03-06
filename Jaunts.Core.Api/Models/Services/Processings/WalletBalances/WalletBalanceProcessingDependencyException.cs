// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.WalletBalance.Exceptions
{
    public class WalletBalanceProcessingDependencyException : Xeption
    {
        public WalletBalanceProcessingDependencyException(Xeption innerException)
            : base(message: "WalletBalance dependency error occurred, Please contact support", innerException)
        { }
        public WalletBalanceProcessingDependencyException(string message,Xeption innerException)
            : base(message, innerException)
        { }
    }
}
