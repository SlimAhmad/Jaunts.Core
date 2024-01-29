// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments.Exceptions
{
    public class FailedAdvertAttachmentStorageException : Xeption
    {
        public FailedAdvertAttachmentStorageException(Exception innerException)
            : base(message: "Failed AdvertAttachment storage error occurred, please contact support.", innerException)
        { }
        public FailedAdvertAttachmentStorageException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
