// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments.Exceptions
{
    public class PromosOfferAttachmentServiceException : Xeption
    {
        public PromosOfferAttachmentServiceException(Exception innerException)
            : base(message: "PromosOfferAttachment service error occurred, contact support.", innerException) { }
        public PromosOfferAttachmentServiceException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}