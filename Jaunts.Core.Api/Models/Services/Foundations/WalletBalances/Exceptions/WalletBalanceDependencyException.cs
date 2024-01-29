// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.WalletBalances.Exceptions
{
    public class WalletBalanceDependencyException : Xeption
    {
        public WalletBalanceDependencyException(Xeption innerException)
             : base(message: "WalletBalance dependency error occurred, contact support.", innerException) { }
        public WalletBalanceDependencyException(string message,Exception innerException)
         : base(message, innerException) { }
    }
}
