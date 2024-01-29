// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Wallets.Exceptions
{
    public class WalletDependencyValidationException : Xeption
    {
        public WalletDependencyValidationException(Xeption innerException)
            : base(message: "Wallet dependency validation error occurred, fix the errors.", innerException) { }
        public WalletDependencyValidationException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}
