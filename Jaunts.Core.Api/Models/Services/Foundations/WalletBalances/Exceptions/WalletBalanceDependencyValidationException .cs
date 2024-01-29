// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.WalletBalances.Exceptions
{
    public class WalletBalanceDependencyValidationException : Xeption
    {
        public WalletBalanceDependencyValidationException(Xeption innerException)
            : base(message: "WalletBalance dependency validation error occurred, fix the errors.", innerException) { }
        public WalletBalanceDependencyValidationException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}
