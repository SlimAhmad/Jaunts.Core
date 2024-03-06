// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Wallet.Exceptions
{
    public class InvalidWalletProcessingException : Xeption
    {
        public InvalidWalletProcessingException()
            : base(message: "Invalid Wallet, Please correct the errors and try again.") 
        { }
        public InvalidWalletProcessingException(string message)
            : base(message)
        { }
    }
}
