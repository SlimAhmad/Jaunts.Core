// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Wallets.Exceptions
{
    public class WalletServiceException : Xeption
    {
        public WalletServiceException(Xeption innerException)
            : base(message: "Wallet service error occurred, contact support.", innerException) { }
        public WalletServiceException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}