// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.WalletBalances.Exceptions
{
    public class WalletBalanceServiceException : Xeption
    {
        public WalletBalanceServiceException(Xeption innerException)
            : base(message: "WalletBalance service error occurred, contact support.", innerException) { }
        public WalletBalanceServiceException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}