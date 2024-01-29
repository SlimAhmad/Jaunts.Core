// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments.Exceptions
{
    public class AlreadyExistsPromosOfferAttachmentException : Xeption
    {
        public AlreadyExistsPromosOfferAttachmentException(Exception innerException)
            : base(message: "PromosOfferAttachment  with the same id already exists.", innerException) { }
        public AlreadyExistsPromosOfferAttachmentException(string message,Exception innerException)
           : base(message, innerException) { }
    }
}
