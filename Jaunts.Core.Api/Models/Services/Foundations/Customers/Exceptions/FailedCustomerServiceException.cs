// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Customers.Exceptions
{
    public class FailedCustomerServiceException : Xeption
    {
        public FailedCustomerServiceException(Exception innerException)
            : base(message: "Failed Customer service error occurred, Please contact support.",
                  innerException)
        { }
        public FailedCustomerServiceException(string message,Exception innerException)
          : base(message,
                innerException)
        { }
    }
}
