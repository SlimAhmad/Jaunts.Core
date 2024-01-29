// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.WalletBalances.Exceptions
{
    public class FailedWalletBalanceServiceException : Xeption
    {
        public FailedWalletBalanceServiceException(Exception innerException)
            : base(message: "Failed WalletBalance service error occurred, contact support.",
                  innerException)
        { }
        public FailedWalletBalanceServiceException(string message,Exception innerException)
          : base(message,
                innerException)
        { }
    }
}
