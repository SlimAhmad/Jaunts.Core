// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments.Exceptions
{
    public class PromosOfferAttachmentValidationException : Xeption
    {
        public PromosOfferAttachmentValidationException(Exception innerException)
            : base(message: "Invalid input, contact support.", innerException) { }
        public PromosOfferAttachmentValidationException(string message,Exception innerException)
            : base(message, innerException) { }

    }
}