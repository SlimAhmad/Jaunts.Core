// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Wallet.Exceptions
{
    public class NullWalletProcessingException : Xeption
    {
        public NullWalletProcessingException()
            : base(message: "Wallet is null.")
        { }
        public NullWalletProcessingException(string message)
            : base(message)
        { }
    }
}
