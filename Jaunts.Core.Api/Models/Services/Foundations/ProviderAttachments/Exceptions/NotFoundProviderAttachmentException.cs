// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments.Exceptions
{
    public class NotFoundProviderAttachmentException : Xeption
    {

        public NotFoundProviderAttachmentException(Guid packageId, Guid attachmentId)
        : base(message: $"Couldn't find package attachment with student id: {packageId} " +
               $"and attachment id: {attachmentId}.")
        { }
        public NotFoundProviderAttachmentException(string message)
            : base(message) { }
    }
}
