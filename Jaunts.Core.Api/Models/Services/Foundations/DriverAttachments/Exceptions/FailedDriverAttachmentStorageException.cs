// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments.Exceptions
{
    public class FailedDriverAttachmentStorageException : Xeption
    {
        public FailedDriverAttachmentStorageException(Exception innerException)
            : base(message: "Failed DriverAttachment storage error occurred, please contact support.", innerException)
        { }
        public FailedDriverAttachmentStorageException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
