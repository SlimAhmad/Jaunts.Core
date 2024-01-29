// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Customers.Exceptions
{
    public class LockedCustomerException : Xeption
    {
        public LockedCustomerException(Exception innerException)
            : base(message: "Locked Customer record exception, Please try again later.", innerException) { }
        public LockedCustomerException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}
