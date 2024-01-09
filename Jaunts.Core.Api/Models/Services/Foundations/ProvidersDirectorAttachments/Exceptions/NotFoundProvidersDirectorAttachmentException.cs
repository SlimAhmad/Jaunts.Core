// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments.Exceptions
{
    public class NotFoundProvidersDirectorAttachmentException : Xeption
    {

        public NotFoundProvidersDirectorAttachmentException(Guid packageId, Guid attachmentId)
        : base(message: $"Couldn't find package attachment with student id: {packageId} " +
               $"and attachment id: {attachmentId}.")
        { }
        public NotFoundProvidersDirectorAttachmentException(string message)
            : base(message) { }
    }
}
