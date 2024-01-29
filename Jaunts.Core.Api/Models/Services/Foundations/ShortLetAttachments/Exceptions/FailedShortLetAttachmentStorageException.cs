// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments.Exceptions
{
    public class FailedShortLetAttachmentStorageException : Xeption
    {
        public FailedShortLetAttachmentStorageException(Exception innerException)
            : base(message: "Failed ShortLetAttachment storage error occurred, Please contact support.", innerException)
        { }
        public FailedShortLetAttachmentStorageException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
