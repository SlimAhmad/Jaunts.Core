// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Wallets.Exceptions
{
    public class FailedWalletStorageException : Xeption
    {
        public FailedWalletStorageException(Exception innerException)
            : base(message: "Failed Wallet storage error occurred, Please contact support.", innerException)
        { }
        public FailedWalletStorageException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
