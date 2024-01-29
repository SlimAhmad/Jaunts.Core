// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions
{
    public class NullAttachmentException : Exception
    {
        public NullAttachmentException() : base(message: "The attachment is null.") { }

        public NullAttachmentException(string message) : base(message) { }
    }
}
