﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments.Exceptions
{
    public class DriverAttachmentServiceException : Xeption
    {
        public DriverAttachmentServiceException(Exception innerException)
            : base(message: "DriverAttachment service error occurred, contact support.", innerException) { }
        public DriverAttachmentServiceException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}