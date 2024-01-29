// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments.Exceptions
{
    public class PromosOfferAttachmentDependencyValidationException : Xeption
    {
        public PromosOfferAttachmentDependencyValidationException(Xeption innerException)
            : base(message: "PromosOfferAttachment dependency validation error occurred, fix the errors.", innerException) { }
        public PromosOfferAttachmentDependencyValidationException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}
