// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments.Exceptions
{
    public class LockedAdvertAttachmentException : Xeption
    {
        public LockedAdvertAttachmentException(Exception innerException)
            : base(message: "Locked AdvertAttachment record exception, please try again later.", innerException) { }
        public LockedAdvertAttachmentException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}
