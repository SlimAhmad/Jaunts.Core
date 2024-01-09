// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments.Exceptions
{
    public class AlreadyExistsCustomerAttachmentException : Xeption
    {
        public AlreadyExistsCustomerAttachmentException(Exception innerException)
            : base(message: "CustomerAttachment  with the same id already exists.", innerException) { }
        public AlreadyExistsCustomerAttachmentException(string message,Exception innerException)
           : base(message, innerException) { }
    }
}
