﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Customers.Exceptions
{
    public class InvalidCustomerException : Xeption
    {



        public InvalidCustomerException()
            : base(message: "Invalid Customer. Please fix the errors and try again.") { }

        public InvalidCustomerException(string message)
            : base(message)
        { }
    }
}