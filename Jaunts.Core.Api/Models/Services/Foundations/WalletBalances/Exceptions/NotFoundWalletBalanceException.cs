// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.WalletBalances.Exceptions
{
    public class NotFoundWalletBalanceException : Xeption
    {
        public NotFoundWalletBalanceException(Guid WalletBalanceId)
            : base(message: $"Couldn't find WalletBalance with id: {WalletBalanceId}.") { }
        public NotFoundWalletBalanceException(string message)
            : base(message) { }
    }
}
