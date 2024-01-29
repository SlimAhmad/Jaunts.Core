// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Wallets.Exceptions
{
    public class NullWalletException : Xeption
    {
        public NullWalletException() : base(message: "The Wallet is null.") { }
        public NullWalletException(string message) : base(message) { }
    }
}
