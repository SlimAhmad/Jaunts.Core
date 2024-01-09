// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments.Exceptions
{
    public class AlreadyExistsProviderAttachmentException : Xeption
    {
        public AlreadyExistsProviderAttachmentException(Exception innerException)
            : base(message: "ProviderAttachment  with the same id already exists.", innerException) { }
        public AlreadyExistsProviderAttachmentException(string message,Exception innerException)
           : base(message, innerException) { }
    }
}
