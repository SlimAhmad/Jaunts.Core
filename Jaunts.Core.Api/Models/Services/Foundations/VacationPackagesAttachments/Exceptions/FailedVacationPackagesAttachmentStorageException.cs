// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.VacationPackagesAttachments.Exceptions
{
    public class FailedVacationPackagesAttachmentStorageException : Xeption
    {
        public FailedVacationPackagesAttachmentStorageException(Exception innerException)
            : base(message: "Failed VacationPackagesAttachment storage error occurred, please contact support.", innerException)
        { }
        public FailedVacationPackagesAttachmentStorageException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
