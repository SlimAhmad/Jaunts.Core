// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.WalletBalances.Exceptions
{
    public class NullWalletBalanceException : Xeption
    {
        public NullWalletBalanceException() : base(message: "The WalletBalance is null.") { }
        public NullWalletBalanceException(string message) : base(message) { }
    }
}
