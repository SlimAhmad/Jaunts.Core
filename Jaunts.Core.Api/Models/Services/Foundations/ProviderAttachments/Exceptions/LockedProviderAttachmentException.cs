// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments.Exceptions
{
    public class LockedProviderAttachmentException : Xeption
    {
        public LockedProviderAttachmentException(Exception innerException)
            : base(message: "Locked ProviderAttachment record exception, please try again later.", innerException) { }
        public LockedProviderAttachmentException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}
