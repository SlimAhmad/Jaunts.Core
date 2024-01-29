// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments.Exceptions
{
    public class AlreadyExistsPackageAttachmentException : Xeption
    {
        public AlreadyExistsPackageAttachmentException(Exception innerException)
            : base(message: "PackageAttachment  with the same id already exists.", innerException) { }
        public AlreadyExistsPackageAttachmentException(string message,Exception innerException)
           : base(message, innerException) { }
    }
}
