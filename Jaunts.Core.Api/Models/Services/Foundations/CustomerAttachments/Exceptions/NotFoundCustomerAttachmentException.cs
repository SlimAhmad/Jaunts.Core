// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments.Exceptions
{
    public class NotFoundCustomerAttachmentException : Xeption
    {

        public NotFoundCustomerAttachmentException(Guid customerId, Guid attachmentId)
        : base(message: $"Couldn't find attachment with Customer id: {customerId} " +
               $"and attachment id: {attachmentId}.")
        { }
        public NotFoundCustomerAttachmentException(string message)
            : base(message) { }
    }
}
