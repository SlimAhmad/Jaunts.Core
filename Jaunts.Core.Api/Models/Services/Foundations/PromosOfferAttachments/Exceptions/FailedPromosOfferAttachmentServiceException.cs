// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments.Exceptions
{
    public class FailedPromosOfferAttachmentServiceException : Xeption
    {
        public FailedPromosOfferAttachmentServiceException(Exception innerException)
            : base(message: "Failed PromosOfferAttachment service error occurred, please contact support.",
                  innerException)
        { }
        public FailedPromosOfferAttachmentServiceException(string message,Exception innerException)
          : base(message,
                innerException)
        { }
    }
}
