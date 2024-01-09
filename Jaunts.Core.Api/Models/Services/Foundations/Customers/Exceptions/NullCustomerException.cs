// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Customers.Exceptions
{
    public class NullCustomerException : Xeption
    {
        public NullCustomerException() : base(message: "The customer is null.") { }
        public NullCustomerException(string message) : base(message) { }
    }
}
