// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Emails.Exceptions
{
    public class FailedEmailProcessingServiceException : Xeption
    {
        public FailedEmailProcessingServiceException(Exception innerException)
            : base(message: "Failed email processing service occurred, please contact support", innerException)
        { }
        public FailedEmailProcessingServiceException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
