// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Customers.Exceptions
{
    public class FailedCustomerStorageException : Xeption
    {
        public FailedCustomerStorageException(Exception innerException)
            : base(message: "Failed customer storage error occurred, please contact support.", innerException)
        { }
        public FailedCustomerStorageException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
