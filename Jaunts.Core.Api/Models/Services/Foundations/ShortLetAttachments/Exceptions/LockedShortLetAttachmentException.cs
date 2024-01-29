// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments.Exceptions
{
    public class LockedShortLetAttachmentException : Xeption
    {
        public LockedShortLetAttachmentException(Exception innerException)
            : base(message: "Locked ShortLetAttachment record exception, Please try again later.", innerException) { }
        public LockedShortLetAttachmentException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}
