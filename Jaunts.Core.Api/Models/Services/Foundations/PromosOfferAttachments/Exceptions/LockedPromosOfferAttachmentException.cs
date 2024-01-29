// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments.Exceptions
{
    public class LockedPromosOfferAttachmentException : Xeption
    {
        public LockedPromosOfferAttachmentException(Exception innerException)
            : base(message: "Locked PromosOfferAttachment record exception, please try again later.", innerException) { }
        public LockedPromosOfferAttachmentException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}
