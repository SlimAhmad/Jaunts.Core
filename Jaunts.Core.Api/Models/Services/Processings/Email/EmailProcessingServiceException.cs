// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Emails.Exceptions
{
    public class EmailProcessingServiceException : Xeption
    {
        public EmailProcessingServiceException(Exception innerException)
            : base(message: "Failed external Email service occurred, please contact support", innerException)
        { }
    }
}
