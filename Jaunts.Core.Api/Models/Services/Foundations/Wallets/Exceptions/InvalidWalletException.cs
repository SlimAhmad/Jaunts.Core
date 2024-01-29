// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Wallets.Exceptions
{
    public class InvalidWalletException : Xeption
    {
        public InvalidWalletException()
            : base(message: "Invalid Wallet. Please fix the errors and try again.")
        { }

        public InvalidWalletException(string parameterName, object parameterValue)
            : base(message: $"Invalid student, " +
          $"parameter name: {parameterName}, " +
          $"parameter value: {parameterValue}.")
        { }

        public InvalidWalletException(string message)
            : base(message)
        { }
    }
}