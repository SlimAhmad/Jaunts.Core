// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Fleets.Exceptions
{
    public class InvalidFleetException : Xeption
    {
        public InvalidFleetException(string parameterName, object parameterValue)
            : base(message: $"Invalid fleet, " +
          $"parameter name: {parameterName}, " +
          $"parameter value: {parameterValue}.")
        { }

        public InvalidFleetException()
            : base(message: "Invalid fleet. Please fix the errors and try again.")
        { }
        public InvalidFleetException(string message)
            : base(message)
        { }
    }
}