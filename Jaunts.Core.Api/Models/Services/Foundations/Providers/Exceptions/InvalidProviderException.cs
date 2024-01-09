// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Providers.Exceptions
{
    public class InvalidProviderException : Xeption
    {
        public InvalidProviderException(string parameterName, object parameterValue)
            : base(message: $"Invalid provider, " +
          $"parameter name: {parameterName}, " +
          $"parameter value: {parameterValue}.")
        { }

        public InvalidProviderException()
            : base(message: "Invalid provider. Please fix the errors and try again.")
        { }
        public InvalidProviderException(string message)
            : base(message)
        { }
    }
}