// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments.Exceptions
{
    public class FailedCustomerAttachmentServiceException : Xeption
    {
        public FailedCustomerAttachmentServiceException(Exception innerException)
            : base(message: "Failed CustomerAttachment service error occurred, contact support",
                  innerException)
        { }
        public FailedCustomerAttachmentServiceException(string message,Exception innerException)
          : base(message,
                innerException)
        { }
    }
}
