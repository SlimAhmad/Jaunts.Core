// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Wallets.Exceptions
{
    public class WalletDependencyException : Xeption
    {
        public WalletDependencyException(Xeption innerException)
             : base(message: "Wallet dependency error occurred, contact support.", innerException) { }
        public WalletDependencyException(string message,Exception innerException)
         : base(message, innerException) { }
    }
}
