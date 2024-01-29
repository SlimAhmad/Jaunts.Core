// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments.Exceptions
{
    public class LockedCustomerAttachmentException : Xeption
    {
        public LockedCustomerAttachmentException(Exception innerException)
            : base(message: "Locked CustomerAttachment record exception, Please try again later.", innerException) { }
        public LockedCustomerAttachmentException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}
