﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions
{
    public class AlreadyExistsAttachmentException : Exception
    {
        public AlreadyExistsAttachmentException(Exception innerException)
            : base(message: "Attachment with the same id already exists.", innerException) { }
        public AlreadyExistsAttachmentException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}
