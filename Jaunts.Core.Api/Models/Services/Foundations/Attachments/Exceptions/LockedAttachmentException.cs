// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions
{
    public class LockedAttachmentException : Exception
    {
        public LockedAttachmentException(Exception innerException)
            : base(message: "Locked attachment record exception, please try again later.", innerException) { }

        public LockedAttachmentException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}
