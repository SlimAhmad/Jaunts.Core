// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Wallets.Exceptions
{
    public class LockedWalletException : Xeption
    {
        public LockedWalletException(Exception innerException)
            : base(message: "Locked Wallet record exception, Please try again later.", innerException) { }
        public LockedWalletException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}
