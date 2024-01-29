// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Wallets.Exceptions
{
    public class AlreadyExistsWalletException : Xeption
    {
        public AlreadyExistsWalletException(Exception innerException)
            : base(message: "Wallet with the same id already exists.", innerException) { }
        public AlreadyExistsWalletException(string message,Exception innerException)
           : base(message, innerException) { }
    }
}
