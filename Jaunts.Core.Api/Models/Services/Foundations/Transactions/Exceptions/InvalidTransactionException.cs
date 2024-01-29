// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Transactions.Exceptions
{
    public class InvalidTransactionException : Xeption
    {
        public InvalidTransactionException(string parameterName, object parameterValue)
         : base(message: $"Invalid Transaction, " +
          $"parameter name: {parameterName}, " +
          $"parameter value: {parameterValue}.")
        { }
        public InvalidTransactionException()
            : base(message: "Invalid Transaction. Please fix the errors and try again.")
        { }
        public InvalidTransactionException(string message)
            : base(message)
        { }
    }
}