// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments.Exceptions
{
    public class AlreadyExistsShortLetAttachmentException : Xeption
    {
        public AlreadyExistsShortLetAttachmentException(Exception innerException)
            : base(message: "ShortLetAttachment  with the same id already exists.", innerException) { }
        public AlreadyExistsShortLetAttachmentException(string message,Exception innerException)
           : base(message, innerException) { }
    }
}
