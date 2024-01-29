// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.WalletBalances.Exceptions
{
    public class AlreadyExistsWalletBalanceException : Xeption
    {
        public AlreadyExistsWalletBalanceException(Exception innerException)
            : base(message: "WalletBalance with the same id already exists.", innerException) { }
        public AlreadyExistsWalletBalanceException(string message,Exception innerException)
           : base(message, innerException) { }
    }
}
