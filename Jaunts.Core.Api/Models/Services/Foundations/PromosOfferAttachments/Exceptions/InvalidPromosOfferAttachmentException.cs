// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments.Exceptions
{
    public class InvalidPromosOfferAttachmentException : Xeption
    {
        public InvalidPromosOfferAttachmentException()
           : base(message: "Invalid PromosOfferAttachment. Please correct the errors and try again.")
        { }

        public InvalidPromosOfferAttachmentException(string message)
            : base(message)
        { }
    }
}