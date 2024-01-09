// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.VacationPackagesAttachments.Exceptions
{
    public class FailedVacationPackagesAttachmentServiceException : Xeption
    {
        public FailedVacationPackagesAttachmentServiceException(Exception innerException)
            : base(message: "Failed VacationPackagesAttachment service error occurred, contact support",
                  innerException)
        { }
        public FailedVacationPackagesAttachmentServiceException(string message,Exception innerException)
          : base(message,
                innerException)
        { }
    }
}
