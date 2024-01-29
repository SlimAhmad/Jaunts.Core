// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments.Exceptions
{
    public class PromosOfferAttachmentDependencyException : Xeption
    {
        public PromosOfferAttachmentDependencyException(Exception innerException)
             : base(message: "PromosOfferAttachment dependency error occurred, contact support.", innerException: innerException) { }
        public PromosOfferAttachmentDependencyException(string message,Exception innerException)
         : base(message, innerException) { }
    }
}
