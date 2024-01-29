// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments.Exceptions
{
    public class NullShortLetAttachmentException : Xeption
    {
        public NullShortLetAttachmentException() : base(message: "The ShortLetAttachment is null.") { }
        public NullShortLetAttachmentException(string message) : base(message) { }
    }
}
