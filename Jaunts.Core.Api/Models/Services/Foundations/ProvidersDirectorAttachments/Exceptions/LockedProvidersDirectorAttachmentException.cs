// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments.Exceptions
{
    public class LockedProvidersDirectorAttachmentException : Xeption
    {
        public LockedProvidersDirectorAttachmentException(Exception innerException)
            : base(message: "Locked ProvidersDirectorAttachment record exception, Please try again later.", innerException) { }
        public LockedProvidersDirectorAttachmentException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}
