// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Customers.Exceptions
{
    public class InvalidCustomerException : Xeption
    {

        public InvalidCustomerException(string parameterName, object parameterValue)
            : base(message: $"Invalid customer, " +
          $"parameter name: {parameterName}, " +
          $"parameter value: {parameterValue}.")
        { }

        public InvalidCustomerException()
            : base(message: "Invalid customer. Please fix the errors and try again.") { }

        public InvalidCustomerException(string message)
            : base(message)
        { }
    }
}