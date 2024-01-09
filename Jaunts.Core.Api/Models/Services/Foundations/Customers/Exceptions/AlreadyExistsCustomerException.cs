// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Customers.Exceptions
{
    public class AlreadyExistsCustomerException : Xeption
    {
        public AlreadyExistsCustomerException(Exception innerException)
            : base(message: "Customer with the same id already exists.", innerException) { }
        public AlreadyExistsCustomerException(string message,Exception innerException)
           : base(message, innerException) { }
    }
}
