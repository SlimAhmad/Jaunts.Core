// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions
{
    public class AttachmentValidationException : Exception
    {
        public AttachmentValidationException(Exception innerException)
            : base(message: "Attachment validation error occurred, please try again.", innerException) { }
        public AttachmentValidationException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}
