// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments.Exceptions
{
    public class LockedDriverAttachmentException : Xeption
    {
        public LockedDriverAttachmentException(Exception innerException)
            : base(message: "Locked DriverAttachment record exception, please try again later.", innerException) { }
        public LockedDriverAttachmentException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}
