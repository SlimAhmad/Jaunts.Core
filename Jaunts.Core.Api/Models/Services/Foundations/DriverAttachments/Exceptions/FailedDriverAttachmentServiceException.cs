// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments.Exceptions
{
    public class FailedDriverAttachmentServiceException : Xeption
    {
        public FailedDriverAttachmentServiceException(Exception innerException)
            : base(message: "Failed DriverAttachment service error occurred, please contact support.",
                  innerException)
        { }
        public FailedDriverAttachmentServiceException(string message,Exception innerException)
          : base(message,
                innerException)
        { }
    }
}
