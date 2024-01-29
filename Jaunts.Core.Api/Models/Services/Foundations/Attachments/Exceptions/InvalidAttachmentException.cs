// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions
{
    public class InvalidAttachmentException : Xeption
    {
        public InvalidAttachmentException()
            : base(message: "Invalid attachment. Please correct the errors and try again.")
        { }

        public InvalidAttachmentException(string message)
            : base(message)
        { }
    }
}
