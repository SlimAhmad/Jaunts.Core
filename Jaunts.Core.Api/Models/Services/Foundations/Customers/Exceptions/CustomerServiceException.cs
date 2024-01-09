// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Customers.Exceptions
{
    public class CustomerServiceException : Xeption
    {
        public CustomerServiceException(Xeption innerException)
            : base(message: "Customer service error occurred, contact support.", innerException) { }
        public CustomerServiceException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}