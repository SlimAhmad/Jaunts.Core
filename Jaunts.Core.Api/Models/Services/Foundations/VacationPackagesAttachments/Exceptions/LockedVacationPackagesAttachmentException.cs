// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments.Exceptions
{
    public class LockedPackageAttachmentException : Xeption
    {
        public LockedPackageAttachmentException(Exception innerException)
            : base(message: "Locked PackageAttachment record exception, Please try again later.", innerException) { }
        public LockedPackageAttachmentException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}
