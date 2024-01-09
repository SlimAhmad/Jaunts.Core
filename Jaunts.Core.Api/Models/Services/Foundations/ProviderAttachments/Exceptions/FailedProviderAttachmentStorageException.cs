// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments.Exceptions
{
    public class FailedProviderAttachmentStorageException : Xeption
    {
        public FailedProviderAttachmentStorageException(Exception innerException)
            : base(message: "Failed ProviderAttachment storage error occurred, please contact support.", innerException)
        { }
        public FailedProviderAttachmentStorageException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
