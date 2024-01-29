// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions
{
    public class AttachmentServiceException : Exception
    {
        public AttachmentServiceException(Exception innerException)
            : base(message: "Attachment Service error occurred, contact support.", innerException) { }
        public AttachmentServiceException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}