// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.WalletBalance.Exceptions
{
    public class NullWalletBalanceProcessingException : Xeption
    {
        public NullWalletBalanceProcessingException()
            : base(message: "WalletBalance is null.")
        { }
        public NullWalletBalanceProcessingException(string message)
            : base(message)
        { }
    }
}
