// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments.Exceptions
{
    public class NotFoundAdvertAttachmentException : Xeption
    {

        public NotFoundAdvertAttachmentException(Guid packageId, Guid attachmentId)
        : base(message: $"Couldn't find attachment with Advert id: {packageId} " +
               $"and attachment id: {attachmentId}.")
        { }
        public NotFoundAdvertAttachmentException(string message)
            : base(message) { }
    }
}
