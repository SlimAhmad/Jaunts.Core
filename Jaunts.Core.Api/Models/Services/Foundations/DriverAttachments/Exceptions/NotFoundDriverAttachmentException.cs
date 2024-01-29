// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments.Exceptions
{
    public class NotFoundDriverAttachmentException : Xeption
    {

        public NotFoundDriverAttachmentException(Guid packageId, Guid attachmentId)
        : base(message: $"Couldn't find attachment with Driver id: {packageId} " +
               $"and attachment id: {attachmentId}.")
        { }
        public NotFoundDriverAttachmentException(string message)
            : base(message) { }
    }
}
