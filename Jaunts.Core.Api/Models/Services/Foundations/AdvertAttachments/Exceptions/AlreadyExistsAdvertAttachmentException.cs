// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments.Exceptions
{
    public class AlreadyExistsAdvertAttachmentException : Xeption
    {
        public AlreadyExistsAdvertAttachmentException(Exception innerException)
            : base(message: "AdvertAttachment  with the same id already exists.", innerException) { }
        public AlreadyExistsAdvertAttachmentException(string message,Exception innerException)
           : base(message, innerException) { }
    }
}
