﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments.Exceptions
{
    public class AdvertAttachmentDependencyValidationException : Xeption
    {
        public AdvertAttachmentDependencyValidationException(Xeption innerException)
            : base(message: "AdvertAttachment dependency validation error occurred, fix the errors.", innerException) { }
        public AdvertAttachmentDependencyValidationException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}
