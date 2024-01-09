// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments.Exceptions
{
    public class NullProviderAttachmentException : Xeption
    {
        public NullProviderAttachmentException() : base(message: "The ProviderAttachment is null.") { }
        public NullProviderAttachmentException(string message) : base(message) { }
    }
}
