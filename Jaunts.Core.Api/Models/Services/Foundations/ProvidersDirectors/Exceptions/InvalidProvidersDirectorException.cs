// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors.Exceptions
{
    public class InvalidProvidersDirectorException : Xeption
    {
        public InvalidProvidersDirectorException(string parameterName, object parameterValue)
    : base(message: $"Invalid providersDirector, " +
          $"parameter name: {parameterName}, " +
          $"parameter value: {parameterValue}.")
        { }

        public InvalidProvidersDirectorException()
            : base(message: "Invalid providersDirector. Please fix the errors and try again.") { }

        public InvalidProvidersDirectorException(string message)
            : base(message)
        { }
    }
}