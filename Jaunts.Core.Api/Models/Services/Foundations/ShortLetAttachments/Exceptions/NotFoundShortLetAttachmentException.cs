// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments.Exceptions
{
    public class NotFoundShortLetAttachmentException : Xeption
    {

        public NotFoundShortLetAttachmentException(Guid packageId, Guid attachmentId)
        : base(message: $"Couldn't find attachment with ShortLet id: {packageId} " +
               $"and attachment id: {attachmentId}.")
        { }
        public NotFoundShortLetAttachmentException(string message)
            : base(message) { }
    }
}
