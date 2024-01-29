// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments.Exceptions
{
    public class FailedPackageAttachmentServiceException : Xeption
    {
        public FailedPackageAttachmentServiceException(Exception innerException)
            : base(message: "Failed PackageAttachment service error occurred, Please contact support.",
                  innerException)
        { }
        public FailedPackageAttachmentServiceException(string message,Exception innerException)
          : base(message,
                innerException)
        { }
    }
}
