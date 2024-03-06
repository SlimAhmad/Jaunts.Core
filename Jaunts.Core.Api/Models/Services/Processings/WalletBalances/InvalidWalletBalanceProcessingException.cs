// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.WalletBalance.Exceptions
{
    public class InvalidWalletBalanceProcessingException : Xeption
    {
        public InvalidWalletBalanceProcessingException()
            : base(message: "Invalid WalletBalance, Please correct the errors and try again.") 
        { }
        public InvalidWalletBalanceProcessingException(string message)
            : base(message)
        { }
    }
}
