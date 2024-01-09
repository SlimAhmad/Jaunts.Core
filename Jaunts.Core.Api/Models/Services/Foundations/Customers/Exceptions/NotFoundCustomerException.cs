// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Customers.Exceptions
{
    public class NotFoundCustomerException : Xeption
    {
        public NotFoundCustomerException(Guid customerId)
            : base(message: $"Couldn't find customer with id: {customerId}.") { }
        public NotFoundCustomerException(string message)
            : base(message) { }
    }
}
