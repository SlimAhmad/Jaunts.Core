// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments.Exceptions
{
    public class FailedProvidersDirectorAttachmentStorageException : Xeption
    {
        public FailedProvidersDirectorAttachmentStorageException(Exception innerException)
            : base(message: "Failed ProvidersDirectorAttachment storage error occurred, Please contact support.", innerException)
        { }
        public FailedProvidersDirectorAttachmentStorageException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
