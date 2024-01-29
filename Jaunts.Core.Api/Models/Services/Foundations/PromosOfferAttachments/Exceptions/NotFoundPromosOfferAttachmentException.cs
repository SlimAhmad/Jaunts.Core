// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments.Exceptions
{
    public class NotFoundPromosOfferAttachmentException : Xeption
    {

        public NotFoundPromosOfferAttachmentException(Guid packageId, Guid attachmentId)
        : base(message: $"Couldn't find attachment with PromosOffer id: {packageId} " +
               $"and attachment id: {attachmentId}.")
        { }
        public NotFoundPromosOfferAttachmentException(string message)
            : base(message) { }
    }
}
