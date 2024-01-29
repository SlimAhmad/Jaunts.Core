// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Wallets.Exceptions
{
    public class NotFoundWalletException : Xeption
    {
        public NotFoundWalletException(Guid WalletId)
            : base(message: $"Couldn't find Wallet with id: {WalletId}.") { }
        public NotFoundWalletException(string message)
            : base(message) { }
    }
}
