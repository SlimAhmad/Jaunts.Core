// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.WalletBalances.Exceptions
{
    public class WalletBalanceValidationException : Xeption
    {
        public WalletBalanceValidationException(Xeption innerException)
            : base(message: "WalletBalance validation error occurred, Please try again.", innerException) { }
        public WalletBalanceValidationException(string message,Xeption innerException)
            : base(message, innerException) { }

    }
}