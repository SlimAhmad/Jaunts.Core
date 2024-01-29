// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Amenitys.Exceptions
{
    public class InvalidAmenityException : Xeption
    {
        public InvalidAmenityException(string parameterName, object parameterValue)
         : base(message: $"Invalid Amenity, " +
          $"parameter name: {parameterName}, " +
          $"parameter value: {parameterValue}.")
        { }
        public InvalidAmenityException()
            : base(message: "Invalid Amenity. Please fix the errors and try again.")
        { }
        public InvalidAmenityException(string message)
            : base(message)
        { }
    }
}