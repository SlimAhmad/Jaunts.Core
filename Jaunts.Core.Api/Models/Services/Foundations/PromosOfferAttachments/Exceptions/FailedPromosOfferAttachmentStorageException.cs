// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments.Exceptions
{
    public class FailedPromosOfferAttachmentStorageException : Xeption
    {
        public FailedPromosOfferAttachmentStorageException(Exception innerException)
            : base(message: "Failed PromosOfferAttachment storage error occurred, please contact support.", innerException)
        { }
        public FailedPromosOfferAttachmentStorageException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
