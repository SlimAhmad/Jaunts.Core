// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments.Exceptions
{
    public class AlreadyExistsDriverAttachmentException : Xeption
    {
        public AlreadyExistsDriverAttachmentException(Exception innerException)
            : base(message: "DriverAttachment  with the same id already exists.", innerException) { }
        public AlreadyExistsDriverAttachmentException(string message,Exception innerException)
           : base(message, innerException) { }
    }
}
