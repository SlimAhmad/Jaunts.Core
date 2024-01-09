// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions
{
    public class NotFoundAttachmentException : Exception
    {
        public NotFoundAttachmentException(Guid attachmentId)
            : base(message: $"Couldn't find attachment with id: {attachmentId}.") { }
    }
}
