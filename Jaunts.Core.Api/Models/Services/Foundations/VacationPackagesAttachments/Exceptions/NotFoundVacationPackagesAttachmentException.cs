// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments.Exceptions
{
    public class NotFoundPackageAttachmentException : Xeption
    {

        public NotFoundPackageAttachmentException(Guid packageId, Guid attachmentId)
        : base(message: $"Couldn't find attachment with Package id: {packageId} " +
               $"and attachment id: {attachmentId}.")
        { }
        public NotFoundPackageAttachmentException(string message)
            : base(message) { }
    }
}
