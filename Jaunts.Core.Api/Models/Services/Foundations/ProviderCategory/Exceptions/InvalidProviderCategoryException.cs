// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderCategorys.Exceptions
{
    public class InvalidProviderCategoryException : Xeption
    {
        public InvalidProviderCategoryException(string parameterName, object parameterValue)
            : base(message: $"Invalid providerCategory, " +
          $"parameter name: {parameterName}, " +
          $"parameter value: {parameterValue}.")
        { }

        public InvalidProviderCategoryException()
            : base(message: "Invalid providerCategory. Please fix the errors and try again.") { }

        public InvalidProviderCategoryException(string message)
            : base(message)
        { }
    }
}