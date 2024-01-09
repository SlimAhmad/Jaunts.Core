// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions
{
    public class FailedAttachmentServiceException : Xeption
    {
        public FailedAttachmentServiceException(Exception innerException)
            : base(message: "Failed Attachment Service Exception occured,contact support", innerException)
        { }
    }
}
