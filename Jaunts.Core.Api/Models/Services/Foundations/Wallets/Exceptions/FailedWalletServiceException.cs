// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Wallets.Exceptions
{
    public class FailedWalletServiceException : Xeption
    {
        public FailedWalletServiceException(Exception innerException)
            : base(message: "Failed Wallet service error occurred, contact support.",
                  innerException)
        { }
        public FailedWalletServiceException(string message,Exception innerException)
          : base(message,
                innerException)
        { }
    }
}
