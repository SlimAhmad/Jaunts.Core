// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments.Exceptions
{
    public class FailedCustomerAttachmentStorageException : Xeption
    {
        public FailedCustomerAttachmentStorageException(Exception innerException)
            : base(message: "Failed CustomerAttachment storage error occurred, Please contact support.", innerException)
        { }
        public FailedCustomerAttachmentStorageException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
