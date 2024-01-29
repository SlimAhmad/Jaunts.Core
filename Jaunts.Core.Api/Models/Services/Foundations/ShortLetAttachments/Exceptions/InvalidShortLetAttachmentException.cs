// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments.Exceptions
{
    public class InvalidShortLetAttachmentException : Xeption
    {
        public InvalidShortLetAttachmentException()
         : base(message: $"Invalid ShortLetAttachment. Please fix the errors and try again.")
        { }

        public InvalidShortLetAttachmentException(string message)
            : base(message)
        { }
    }
}