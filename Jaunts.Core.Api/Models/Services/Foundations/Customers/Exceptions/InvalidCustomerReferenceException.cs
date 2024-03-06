// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Customers.Exceptions
{
    public class InvalidCustomerReferenceException : Xeption
    {
        public InvalidCustomerReferenceException(Exception innerException)
            : base(message: "Invalid customer reference error occurred.", innerException)
        { }
        public InvalidCustomerReferenceException(string message)
            : base(message)
        { }
    }
}
