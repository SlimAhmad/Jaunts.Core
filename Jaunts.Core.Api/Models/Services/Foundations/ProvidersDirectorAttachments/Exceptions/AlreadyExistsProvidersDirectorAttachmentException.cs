// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments.Exceptions
{
    public class AlreadyExistsProvidersDirectorAttachmentException : Xeption
    {
        public AlreadyExistsProvidersDirectorAttachmentException(Exception innerException)
            : base(message: "ProvidersDirectorAttachment  with the same id already exists.", innerException) { }
        public AlreadyExistsProvidersDirectorAttachmentException(string message,Exception innerException)
           : base(message, innerException) { }
    }
}
