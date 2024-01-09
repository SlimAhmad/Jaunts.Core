// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments.Exceptions
{
    public class FailedProviderAttachmentServiceException : Xeption
    {
        public FailedProviderAttachmentServiceException(Exception innerException)
            : base(message: "Failed ProviderAttachment service error occurred, contact support",
                  innerException)
        { }
        public FailedProviderAttachmentServiceException(string message,Exception innerException)
          : base(message,
                innerException)
        { }
    }
}
