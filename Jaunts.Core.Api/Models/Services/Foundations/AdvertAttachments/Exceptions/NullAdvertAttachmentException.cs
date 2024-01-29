// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments.Exceptions
{
    public class NullAdvertAttachmentException : Xeption
    {
        public NullAdvertAttachmentException() : base(message: "The AdvertAttachment is null.") { }
        public NullAdvertAttachmentException(string message) : base(message) { }
    }
}
