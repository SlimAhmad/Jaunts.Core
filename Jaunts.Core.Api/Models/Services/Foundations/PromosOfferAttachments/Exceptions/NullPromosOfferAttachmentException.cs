// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments.Exceptions
{
    public class NullPromosOfferAttachmentException : Xeption
    {
        public NullPromosOfferAttachmentException() : base(message: "The PromosOfferAttachment is null.") { }
        public NullPromosOfferAttachmentException(string message) : base(message) { }
    }
}
