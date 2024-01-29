// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments.Exceptions
{
    public class FailedShortLetAttachmentServiceException : Xeption
    {
        public FailedShortLetAttachmentServiceException(Exception innerException)
            : base(message: "Failed ShortLetAttachment service error occurred, Please contact support.",
                  innerException)
        { }
        public FailedShortLetAttachmentServiceException(string message,Exception innerException)
          : base(message,
                innerException)
        { }
    }
}
