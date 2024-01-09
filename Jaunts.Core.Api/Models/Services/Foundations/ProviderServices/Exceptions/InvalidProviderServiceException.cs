// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderServices.Exceptions
{
    public class InvalidProviderServiceException : Xeption
    {
        public InvalidProviderServiceException(string parameterName, object parameterValue)
         : base(message: $"Invalid providerService, " +
          $"parameter name: {parameterName}, " +
          $"parameter value: {parameterValue}.")
        { }

        public InvalidProviderServiceException()
            : base(message: "Invalid providerService. Please fix the errors and try again.") { }

        public InvalidProviderServiceException(string message)
            : base(message)
        { }
    }
}