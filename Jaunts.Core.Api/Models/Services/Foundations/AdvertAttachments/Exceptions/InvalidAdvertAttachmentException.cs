// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments.Exceptions
{
    public class InvalidAdvertAttachmentException : Xeption
    {
        public InvalidAdvertAttachmentException()
           : base(message: "Invalid AdvertAttachment. Please correct the errors and try again.")
        { }

        public InvalidAdvertAttachmentException(string message)
            : base(message)
        { }
    }
}