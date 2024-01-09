// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments.Exceptions
{
    public class FailedProvidersDirectorAttachmentServiceException : Xeption
    {
        public FailedProvidersDirectorAttachmentServiceException(Exception innerException)
            : base(message: "Failed ProvidersDirectorAttachment service error occurred, contact support",
                  innerException)
        { }
        public FailedProvidersDirectorAttachmentServiceException(string message,Exception innerException)
          : base(message,
                innerException)
        { }
    }
}
