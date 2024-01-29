// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.WalletBalances.Exceptions
{
    public class InvalidWalletBalanceException : Xeption
    {
        public InvalidWalletBalanceException()
            : base(message: "Invalid WalletBalance. Please fix the errors and try again.")
        { }

        public InvalidWalletBalanceException(string parameterName, object parameterValue)
            : base(message: $"Invalid student, " +
          $"parameter name: {parameterName}, " +
          $"parameter value: {parameterValue}.")
        { }

        public InvalidWalletBalanceException(string message)
            : base(message)
        { }
    }
}